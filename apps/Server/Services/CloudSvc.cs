using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Server.LibNS.ErrNS;
using Server.TypesNS.CloudNS;
using Microsoft.AspNetCore.Http;
using Server.ConfigNS.CloudNS;
using Server.LibNS.FilesNS;

namespace Server.ServicesNS.CLoudNS;



public static class CloudSvc
{
  private static async Task<CloudResultDto> UploadImg(IFormFile file, string folder)
  {
    using Stream stream =
 file.OpenReadStream();

    ImageUploadParams uploadParams =
              new()
              {
                File = new FileDescription(
                      FilesLib.MakeFilename(file),
                      stream
                  ),
                Folder = $"cs__{folder}"
              };

    var Connection = CloudConf.Connection;
    ImageUploadResult result =
await Connection.UploadAsync(
uploadParams
);

    return new CloudResultDto
    {
      PublicId = result.PublicId,
      Url = result.Url.ToString()
    };
  }

  private async static Task<CloudResultDto> UploadVideo(IFormFile file, string folder)
  {

    var tempDir = FilesLib.ChainCurrDir("temp");
    var fileName = FilesLib.MakeFilename(file);
    var tempPath = Path.Combine(
        tempDir,
      fileName
    );

    using (var stream = new FileStream(tempPath, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    VideoUploadParams uploadParamsVideo = new()
    {
      File = new FileDescription(tempPath),
      Folder = $"cs__{folder}"
    };

    var Connection = CloudConf.Connection;
    VideoUploadResult result =
        await Connection.UploadAsync(uploadParamsVideo);
    File.Delete(tempPath);

    return new CloudResultDto
    {
      PublicId = result.PublicId,
      Url = result.Url.ToString()
    };
  }

  public static async Task<CloudResultDto> UploadSingle(IFormFile file, string folder)
  {
    if (file.ContentType.StartsWith("video/"))
      return await UploadVideo(file, folder);
    else if (file.ContentType.StartsWith("image/"))
      return await UploadImg(file, folder);
    else
      throw new ErrApp("Unsupported file type");
  }


  public static async Task<
    List<CloudResultDto>
> UploadMultiple(
    List<IFormFile> files,
    string folder
)
  {
    List<CloudResultDto> results =
        [];

    foreach (IFormFile file in files)
    {
      results.Add(await UploadSingle(file, folder));
    }

    return results;
  }

  public static async Task Delete(
  string publicId, string resourceType
)
  {
    DeletionParams deleteParams =
        new(publicId
)
        {
          ResourceType =
                resourceType == "video"
                    ? ResourceType.Video
                    : ResourceType.Image
        };

    var Connection = CloudConf.Connection;
    DeletionResult result =
        await Connection.DestroyAsync(
            deleteParams
        );

    if (result.Result != "ok")
    {
      throw new ErrApp(
          "Failed to delete file"
      );
    }
  }
}