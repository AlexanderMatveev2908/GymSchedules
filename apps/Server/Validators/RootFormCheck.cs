using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Server.LibNS;

namespace Server.ValidatorsNS.FormNS;

public static class RootFormCheck
{
  public static async Task<IResult?> Check<T>(HttpContext ctx)
    where T : notnull, new()
  {
    if (!ctx.Request.HasFormContentType)
      return Res.Json(415, "Content-Type must be multipart/form-data");

    IFormCollection form =
      await ctx.Request.ReadFormAsync();

    T dto = FormToDto<T>(form);

    List<ValidationResult> errors = [];

    bool isValid = Validator.TryValidateObject(
      dto,
      new ValidationContext(dto),
      errors,
      validateAllProperties: true
    );

    if (!isValid)
    {
      var formattedErrors = errors.Select(err => new
      {
        Field = err.MemberNames.FirstOrDefault(),
        Message = err.ErrorMessage
      });

      return Res.Json(400, "Validation failed", new
      {
        errors = formattedErrors
      });
    }

    ctx.Items["dto"] = dto;
    ctx.Items["form"] = form;

    return null;
  }

  private static T FormToDto<T>(IFormCollection form)
    where T : new()
  {
    T dto = new();

    foreach (PropertyInfo prop in typeof(T).GetProperties())
    {
      if (!prop.CanWrite)
        continue;

      string key = ToCamelCase(prop.Name);

      if (!form.TryGetValue(key, out var value))
        continue;

      string rawValue = value.ToString();

      if (string.IsNullOrWhiteSpace(rawValue))
        continue;

      Type targetType =
        Nullable.GetUnderlyingType(prop.PropertyType)
        ?? prop.PropertyType;

      object? converted =
        Convert.ChangeType(rawValue, targetType);

      prop.SetValue(dto, converted);
    }

    return dto;
  }

  private static string ToCamelCase(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return value;

    return char.ToLowerInvariant(value[0]) + value[1..];
  }
}