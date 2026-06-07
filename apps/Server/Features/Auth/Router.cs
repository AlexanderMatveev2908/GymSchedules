using Server.TypesNS.UserNS;
using Server.ExtensionsNS.RootNS;
using Server.FilterNS.JwtND;
using Server.ExtensionsNS.JwtNS;
using Server.TypesNS.AuthNS;

namespace Server.FeaturesNS.AuthNS;

public static class AuthRouter
{
  public static void MapApi(RouteGroupBuilder api)
  {
    api.MapPost("/auth/register", AuthPostCtrl.Register).WithBodyChecked<RegisterDto>();
    api.MapPost("/auth/login", AuthPostCtrl.Login).WithBodyChecked<LoginDto>();
    api.MapGet("/auth/protected", (Delegate)AuthGetCtrl.GetProtected).WithJwtCheck();
    api.MapPost("/auth/refresh", AuthPostCtrl.RefreshToken);

  }
}