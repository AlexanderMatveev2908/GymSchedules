using Server.ConfigNS.SqlNS;
using Server.LibNS;
using Server.LibNS.ShapeNS;

namespace Server.FeaturesNS.UserNS;

public static class UserGetCtrl
{
  public static async Task<IResult> GetUser(HttpContext ctx, SqlDbCtx db)
  {

    var dict = LibShape.ToDict(ctx.Items["user"]);
    var withoutPwd = LibShape.RemoveKeys(dict, "password");

    return Res.Json(200, "user found", new
    {
      user = withoutPwd
    });
  }
}