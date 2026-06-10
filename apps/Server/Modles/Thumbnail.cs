using Server.ModelsNS.UserNS;

namespace Server.ModelsNS.ThumbNS;

public class Thumbnail
{
  public int Id { get; set; }

  public string publicId { get; set; } = null!;

  public string url { get; set; } = null!;

  public int userId { get; set; }
  public User User { get; set; } = null!;
}