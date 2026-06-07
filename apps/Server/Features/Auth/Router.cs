using Server.TypesNS.UserNS;
using Server.ExtensionsNS.RootNS;

namespace Server.FeaturesNS.AuthNS;

public static class AuthRouter
{
  public static void MapApi(RouteGroupBuilder api)
  {
    api.MapPost("/auth/register", AuthCtrl.Register).WithBodyChecked<UserDto>();
  }
}