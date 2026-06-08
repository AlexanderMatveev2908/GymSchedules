using Server.ConfigNS.SqlNS;
using Server.LibNS;

namespace Server.FeaturesNS.UserNS;

public static class UserGetCtrl
{
  public static async Task<IResult> GetUser(HttpContext ctx, SqlDbCtx db)
  {

    return Res.Json(200, "user found");
  }
}