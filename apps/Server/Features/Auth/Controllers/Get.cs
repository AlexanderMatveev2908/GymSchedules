using System.Security.Claims;
using Server.LibNS;

namespace Server.FeaturesNS.AuthNS;

public static class AuthGetCtrl
{
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