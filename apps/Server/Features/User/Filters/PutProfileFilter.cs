using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Server.LibNS;
using Server.LibNS.RegNS;
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
    PutProfileDto dto = (PutProfileDto)httpCtx.Items["dto"]!;

    if (
      file is not null &&
      !file.ContentType.StartsWith("image/")
    )
    {
      return Res.Json(400, "File must be an image");
    }

    if (!string.IsNullOrWhiteSpace(dto.imgFile) && !RegLib.IsCloudUrl(dto.imgFile))
    {
      return Res.Json(400, "Image must be uploaded to cloud");
    }

    httpCtx.Items["file"] = file;

    return await next(ctx);
  }
}