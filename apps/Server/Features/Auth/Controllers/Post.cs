using Microsoft.EntityFrameworkCore;
using Server.ConfigNS.SqlNS;
using Server.LibNS;
using Server.LibNS.ShapeNS;
using Server.LibNS.TokensNS;
using Server.ModelsNS.RefreshTokensNS;
using Server.ModelsNS.UserNS;
using Server.TypesNS.UserNS;

namespace Server.FeaturesNS.AuthNS;

public static class AuthPostCtrl
{
  public static async Task<IResult> Register(HttpContext ctx, SqlDbCtx db)
  {
    UserDto dto = (UserDto)ctx.Items["dto"]!;

    User? existing = await db.User.FirstOrDefaultAsync(
      us => us.Email == dto.Email
    );

    if (existing is not null)
      return Res.Json(404, "User already exists");

    await using var trx = await db.Database.BeginTransactionAsync();

    try
    {
      User newUser = new(dto);
      newUser.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

      db.User.Add(newUser);
      await db.SaveChangesAsync();

      string refreshToken = RefreshTokensLib.Create();
      RefreshToken dbRefreshToken = new()
      {
        UserId = newUser.Id,
        TokenHash = RefreshTokensLib.Hash(refreshToken),
        // ! to set higher in prod 
        ExpiresAt = DateTime.UtcNow.AddMinutes(60)
      };

      db.RefreshToken.Add(dbRefreshToken);
      await db.SaveChangesAsync();

      string accessToken = JwtLib.Create(newUser);

      ctx.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Lax,
        // ! to set higher in prod 
        Expires = DateTimeOffset.UtcNow.AddMinutes(60)
      });

      await trx.CommitAsync();

      return Res.Json(201, "user registered", new
      {
        newUser = LibShape.Merge(newUser, new { id = newUser.Id }),
        accessToken
      });
    }
    catch
    {
      await trx.RollbackAsync();
      return Res.Json(500, "Register failed");
    }
  }


  public static async Task<IResult> RefreshToken(HttpContext ctx, SqlDbCtx db)
  {
    string? refreshToken =
    ctx.Request.Cookies["refreshToken"];

    if (string.IsNullOrWhiteSpace(refreshToken))
      return Res.Json(401, "REFRESH_TOKEN_MISSING");

    string tokenHash =
    RefreshTokensLib.Hash(refreshToken);

    RefreshToken? dbToken =
         await db.RefreshToken
             .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);

    if (dbToken is null)
      return Res.Json(401, "REFRESH_TOKEN_INVALID");


    if (dbToken.ExpiresAt < DateTime.UtcNow)
      return Res.Json(401, "REFRESH_TOKEN_EXPIRED");


    User? user =
         await db.User.FirstOrDefaultAsync(u => u.Id == dbToken.UserId);

    if (user is null)
      return Res.Json(401, "USER_NOT_FOUND");

    string accessToken =
        JwtLib.Create(user);

    return Res.Json(200, "TOKEN_REFRESHED", new
    {
      accessToken
    });
  }
}