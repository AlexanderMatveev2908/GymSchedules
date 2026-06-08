namespace Server.LibNS.ShapeNS;

public static class LibShape
{
  private static string ToCamelCase(string txt)
  {
    if (string.IsNullOrEmpty(txt))
      return txt;

    return char.ToLowerInvariant(txt[0]) + txt[1..];
  }

  public static Dictionary<string, object?> Merge<T>(T dto, object extra)
  {
    Dictionary<string, object?> dict = [];

    foreach (var p in typeof(T).GetProperties())
    {
      dict[ToCamelCase(p.Name)] = p.GetValue(dto);
    }

    foreach (var p in extra.GetType().GetProperties())
    {
      dict[ToCamelCase(p.Name)] = p.GetValue(extra);
    }

    return dict;
  }

  public static Dictionary<string, object?> RemoveKeys(
    Dictionary<string, object?> dict,
    params string[] keys
)
  {
    foreach (string key in keys)
    {
      dict.Remove(key);
    }

    return dict;
  }
}