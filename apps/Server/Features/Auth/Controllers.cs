using InvoicesApp.LibNS;
using InvoicesApp.LibNS.ShapeNS;
using InvoicesApp.ModelsNS.UsersNS;
using InvoicesApp.TypesNS.UsersNS;
using Microsoft.EntityFrameworkCore;
using Server.ConfigNS.SqlNS;

namespace Server.FeaturesNS.AuthNS;

public static class AuthCtrl
{
  public static async Task<IResult> Register(HttpContext ctx, SqlDbCtx db)
  {
    UsersDto dto = (UsersDto)ctx.Items["dto"]!;

    Users? existing = await db.Users.FirstOrDefaultAsync(
      us => us.Email == dto.Email
    );

    if (existing is not null)
      return Res.Json(404, "User already exists");

    Users newUser = new(dto);

    db.Users.Add(newUser);
    await db.SaveChangesAsync();

    return Res.Json(201, "User registered",
  new
  {
    newUser = LibShape.Merge(newUser, new
    {
      id = newUser.Id
    })
  }
    );
  }
}