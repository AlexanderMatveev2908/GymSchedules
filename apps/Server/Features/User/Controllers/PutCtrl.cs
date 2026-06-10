using Microsoft.EntityFrameworkCore.Storage;
using Server.ConfigNS.SqlNS;
using Server.LibNS;

namespace Server.FeaturesNS.UserNS;

public static class UserPutCtrl
{
  public static async Task<IResult> PutProfile(HttpContext ctx, SqlDbCtx db)
  {
    IFormFile? imgFile = (IFormFile)ctx.Items["file"]!;


    await using IDbContextTransaction trx =
    await db.Database.BeginTransactionAsync();

    try { }
    catch
    {
      await trx.RollbackAsync();
      throw;
    }

    return Res.Json(200, "Profile updated", new
    {
      dto = ctx.Items["dto"],
      filename = imgFile?.FileName
    });
  }
}