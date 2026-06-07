using Server.TypesNS.UserNS;
using Server.ExtensionsNS.RootNS;
using Server.FilterNS.JwtND;

namespace Server.FeaturesNS.AuthNS;

public static class AuthRouter
{
  public static void MapApi(RouteGroupBuilder api)
  {
    api.MapPost("/auth/register", AuthCtrl.Register).WithBodyChecked<UserDto>();
    api.MapGet("/auth/protected", (Delegate)AuthCtrl.GetProtected).AddEndpointFilter<JwtFilter>();
  }
}