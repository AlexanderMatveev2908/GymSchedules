using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Server.LibNS;
using Server.ValidatorsNS.FormNS;

namespace Server.FeaturesNS.UserNS;

public class PutProfileFilter : IEndpointFilter
{
  public async ValueTask<object?> InvokeAsync(
    EndpointFilterInvocationContext ctx,
    EndpointFilterDelegate next
  )
  {
    HttpContext httpCtx = ctx.HttpContext;

    var result = await RootFormCheck.Check<PutProfileDto>(httpCtx);

    if (result is not null)
      return result;

    IFormCollection form =
    (IFormCollection)httpCtx.Items["form"]!;
    IFormFile? file = form.Files["imgFile"];

    if (
      file is not null &&
      !file.ContentType.StartsWith("image/")
    )
    {
      return Res.Json(400, "File must be an image");
    }

    httpCtx.Items["file"] = file;

    return await next(ctx);
  }
}