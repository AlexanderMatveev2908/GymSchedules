using System.Text.Json.Serialization;
using Server.ModelsNS.UserNS;
using Server.TypesNS.CloudNS;

namespace Server.ModelsNS.ThumbNS;

public class Thumbnail
{
  public int Id { get; set; }

  public string PublicId { get; set; } = null!;

  public string Url { get; set; } = null!;

  public int UserId { get; set; }

  [JsonIgnore]
  public User User { get; set; } = null!;


  public Thumbnail() { }

  public Thumbnail(CloudResultDto cloudDto, int UserId)
  {
    this.PublicId = cloudDto.PublicId;
    this.Url = cloudDto.Url;
    this.UserId = UserId;
  }
}