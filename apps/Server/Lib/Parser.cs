using System.Reflection;

namespace Server.LibNS.ParserNS;

public static class ParserLib
{
  public static string ToCamelCase(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return value;

    return char.ToLowerInvariant(value[0]) + value[1..];
  }


  public static T FormToDto<T>(IFormCollection form)
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
}