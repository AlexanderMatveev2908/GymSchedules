using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.MiddlewareNS.LogNS;

namespace Server.MiddlewareNS.RootNS;

public static class RootMdw
{
  public static void MainMdw(WebApplication app)
  {
    app.Use(async (HttpContext ctx, Func<Task> next) =>
    {
      await LogMdw.Log(ctx, next);
    });
  }
}