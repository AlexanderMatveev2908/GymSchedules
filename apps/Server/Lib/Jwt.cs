using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InvoicesApp.ModelsNS.UsersNS;
using Microsoft.IdentityModel.Tokens;
using Server.LibNS.EnvNS;

namespace Server.LibNS.JwtNS;

public static class JwtLib
{
  public static string Create(Users user)
  {
    string secret = EnvVarsLib.Get("JWT_SECRET");

    SymmetricSecurityKey key = new(
         Encoding.UTF8.GetBytes(secret)
     );

    SigningCredentials creds = new(
             key,
             SecurityAlgorithms.HmacSha256
         );

    Claim[] claims =
        [
            new Claim("id", user.Id.ToString()),
            new Claim("email", user.Email)
        ];


    JwtSecurityToken token = new(
        claims: claims,
        expires: DateTime.UtcNow.AddDays(7),
        signingCredentials: creds
    );


    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}