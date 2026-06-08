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

    if (!httpCtx.Request.HasFormContentType)
      return Res.Json(415, "Content-Type must be multipart/form-data");

    IFormCollection form =
      await httpCtx.Request.ReadFormAsync();

    string? firstName = form["firstName"];
    string? lastName = form["lastName"];
    IFormFile? file = form.Files["imgFile"];

    PutProfileDto dto = new()
    {
      FirstName = firstName ?? "",
      LastName = lastName ?? ""
    };

    var result = RootFormCheck.Check(dto);

    if (result is not null)
      return result;

    if (
      file is not null &&
      !file.ContentType.StartsWith("image/")
    )
    {
      return Res.Json(400, "File must be an image");
    }

    httpCtx.Items["file"] = file;
    httpCtx.Items["dto"] = dto;

    return await next(ctx);
  }
}