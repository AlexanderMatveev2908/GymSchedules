using Server.LibNS;
using Server.LibNS.ShapeNS;
using Server.ModelsNS.UserNS;
using Server.TypesNS.UserNS;
using Microsoft.EntityFrameworkCore;
using Server.ConfigNS.SqlNS;
using Server.LibNS.JwtNS;
using Server.LibNS.RefreshTokensSvcNS;
using Server.ModelsNS.RefreshTokensNS;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace Server.FeaturesNS.AuthNS;

public static class AuthCtrl
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
        ExpiresAt = DateTime.UtcNow.AddMinutes(5)
      };

      db.RefreshToken.Add(dbRefreshToken);
      await db.SaveChangesAsync();

      string accessToken = JwtLib.Create(newUser);

      ctx.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Lax,
        Expires = DateTimeOffset.UtcNow.AddMinutes(5)
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

  public static async Task<IResult> GetProtected(HttpContext ctx)
  {

    ClaimsPrincipal user =
    (ClaimsPrincipal)ctx.Items["user"]!;


    // foreach (Claim claim in user.Claims)
    // {
    //   Console.WriteLine($"{claim.Type}: {claim.Value}");
    // }

    return Res.Json(200, "protected data", new
    {
      id = user.FindFirst("id")?.Value,
      email = user.FindFirst("email")?.Value,
      exp = user.FindFirst("exp")?.Value
    });
  }
}