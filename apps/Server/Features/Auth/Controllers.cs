using Server.LibNS;
using Server.LibNS.ShapeNS;
using Server.ModelsNS.UsersNS;
using Server.TypesNS.UsersNS;
using Microsoft.EntityFrameworkCore;
using Server.ConfigNS.SqlNS;
using Server.LibNS.JwtNS;
using Server.LibNS.RefreshTokensSvcNS;
using Server.ModelsNS.RefreshTokensNS;

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

    await using var trx = await db.Database.BeginTransactionAsync();

    try
    {
      Users newUser = new(dto);
      newUser.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

      db.Users.Add(newUser);
      await db.SaveChangesAsync();

      string refreshToken = RefreshTokensLib.Create();
      RefreshTokens dbRefreshToken = new()
      {
        UserId = newUser.Id,
        TokenHash = RefreshTokensLib.Hash(refreshToken),
        ExpiresAt = DateTime.UtcNow.AddDays(7)
      };

      db.RefreshTokens.Add(dbRefreshToken);
      await db.SaveChangesAsync();

      string accessToken = JwtLib.Create(newUser);

      ctx.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Lax,
        Expires = DateTimeOffset.UtcNow.AddDays(7)
      });

      await trx.CommitAsync();

      return Res.Json(201, "user registered", new
      {
        newUser = LibShape.Merge(newUser, new { id = newUser.Id })
      });
    }
    catch
    {
      await trx.RollbackAsync();
      return Res.Json(500, "Register failed");
    }
  }
}