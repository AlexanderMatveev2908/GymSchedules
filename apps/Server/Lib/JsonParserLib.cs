using System.Text.Json;
using Server.LibNS.ErrNS;

namespace InvoicesApp.Lib;

public static class JsonParserLib
{
  public static T? Parse<T>(string json)
  {
    try
    {
      return JsonSerializer.Deserialize<T>(json
      );
    }
    catch (JsonException ex)
    {
      throw new ErrApp($"Invalid JSON format => {ex.Message}");
    }
  }

  public static string Stringify<T>(T obj)
  {
    try
    {
      return JsonSerializer.Serialize(obj,
       new JsonSerializerOptions
       {
         WriteIndented = true,
         PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
       });
    }
    catch (JsonException ex)
    {
      throw new ErrApp($"Error serializing object to JSON => {ex.Message}");
    }
  }
}