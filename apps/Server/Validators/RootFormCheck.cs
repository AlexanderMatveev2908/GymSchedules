using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Server.LibNS;
using Server.LibNS.ParserNS;

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

    T dto = ParserLib.FormToDto<T>(form);

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




}