using System.Text.RegularExpressions;

namespace Server.LibNS.RegNS;

public static class RegLib
{
  public static readonly Regex CloudUrl =
      new(@"^https?://res\.cloudinary\.com/");

  public static bool Check(Regex reg, string txt)
  {
    return reg.IsMatch(txt);
  }

  public static bool IsCloudUrl(string txt)
  {
    return Check(CloudUrl, txt);
  }
}