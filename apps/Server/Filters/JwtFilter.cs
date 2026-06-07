using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.LibNS;

namespace Server.FilterNS.JwtND;

public class JwtFilter : IEndpointFilter
{
  public async ValueTask<object?> InvokeAsync(
   EndpointFilterInvocationContext ctx,
   EndpointFilterDelegate next
)
  {
    HttpContext http = ctx.HttpContext;
    string? auth =
        http.Request.Headers.Authorization.FirstOrDefault();

    if (string.IsNullOrWhiteSpace(auth))
      return Res.Json(401, "JWT_NOT_PROVIDED");

    if (!auth.StartsWith("Bearer "))
      return Res.Json(401, "JWT_INVALID");

    string token = auth["Bearer ".Length..];

    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
    JwtSecurityTokenHandler handler = new();
    byte[] key =
              Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!);


    try
    {
      ClaimsPrincipal user = handler.ValidateToken(
              token,
              new TokenValidationParameters
              {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
              },
              out SecurityToken validatedToken
          );

      http.Items["user"] = user;

      return await next(ctx);
    }
    catch
    {
      return Res.Json(401, "JWT_INVALID");
    }
  }
}