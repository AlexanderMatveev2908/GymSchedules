using InvoicesApp.LibNS;
using InvoicesApp.LibNS.ShapeNS;
using InvoicesApp.ModelsNS.UsersNS;
using InvoicesApp.TypesNS.UsersNS;
using Microsoft.EntityFrameworkCore;
using Server.ConfigNS.SqlNS;
using Server.LibNS.JwtNS;

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
    newUser.Password = BCrypt.Net.BCrypt.HashPassword(
    dto.Password
);

    db.Users.Add(newUser);
    await db.SaveChangesAsync();

    string token = JwtLib.Create(newUser);

    return Res.Json(201, "User registered",
  new
  {
    accessToken = token,
    newUser = LibShape.Merge(newUser, new
    {
      id = newUser.Id
    })
  }
    );
  }
}