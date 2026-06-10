using Server.ConfigNS.SqlNS;
using Server.LibNS;
using Server.LibNS.ShapeNS;
using Server.ModelsNS.ThumbNS;
using Microsoft.EntityFrameworkCore;
using Server.ModelsNS.UserNS;

namespace Server.FeaturesNS.UserNS;

public static class UserGetCtrl
{
  public static async Task<IResult> GetUser(HttpContext ctx, SqlDbCtx db)
  {

    User user = (User)ctx.Items["user"]!;
    var dict = LibShape.ToDict(user);
    var withoutPwd = LibShape.RemoveKeys(dict, "password");

    Thumbnail? thumb =
    await db.Thumbnail
        .FirstOrDefaultAsync(t => t.UserId == user.Id);
    withoutPwd["thumbnail"] = thumb;


    return Res.Json(200, "user found", new
    {
      user = withoutPwd
    });
  }
}