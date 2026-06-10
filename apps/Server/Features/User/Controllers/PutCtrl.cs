using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Server.ConfigNS.SqlNS;
using Server.LibNS;
using Server.ModelsNS.ThumbNS;
using Server.ModelsNS.UserNS;
using Server.ServicesNS.CLoudNS;
using Server.TypesNS.CloudNS;

namespace Server.FeaturesNS.UserNS;

public static class UserPutCtrl
{
  public static async Task<IResult> PutProfile(HttpContext ctx, SqlDbCtx db)
  {
    IFormFile? imgFile = (IFormFile)ctx.Items["file"]!;


    await using IDbContextTransaction trx =
    await db.Database.BeginTransactionAsync();

    string? newPublicId = null;

    try
    {
      User user = (User)ctx.Items["user"]!;

      Thumbnail? thumb =
          await db.Thumbnail.FirstOrDefaultAsync(t => t.UserId == user.Id);

      if (imgFile is not null)
      {
        if (thumb is not null)
        {
          await CloudSvc.Delete(thumb.PublicId, "image");
          db.Thumbnail.Remove(thumb);
        }

        CloudResultDto cloudDto =
            await CloudSvc.UploadSingle(imgFile, "thumbnails");
        newPublicId = cloudDto.PublicId;

        Thumbnail newThumb = new(cloudDto, user.Id);
        db.Thumbnail.Add(newThumb);
      }

      PutProfileDto dtoForm = (PutProfileDto)ctx.Items["dto"]!;
      user.FirstName = dtoForm.FirstName;
      user.LastName = dtoForm.LastName;
      db.User.Update(user);

      await db.SaveChangesAsync();
      await trx.CommitAsync();

      return Res.Json(200, "Profile updated");
    }
    catch
    {
      await trx.RollbackAsync();

      if (newPublicId is not null)
        await CloudSvc.Delete(newPublicId, "image");

      throw;
    }

  }
}