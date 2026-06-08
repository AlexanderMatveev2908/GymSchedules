using Server.ConfigNS.SqlNS;
using Server.LibNS;

namespace Server.FeaturesNS.UserNS;

public static class UserPutCtrl
{
  public static async Task<IResult> PutProfile(HttpContext ctx, SqlDbCtx db)
  {
    IFormFile? imgFile = (IFormFile)ctx.Items["file"]!;


    return Res.Json(200, "Profile updated", new
    {
      dto = ctx.Items["dto"],
      filename = imgFile?.FileName
    });
  }
}