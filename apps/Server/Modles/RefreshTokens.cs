using Server.ModelsNS.UsersNS;

namespace Server.ModelsNS.RefreshTokensNS;

public class RefreshTokens
{
  public int Id { get; set; }

  public int UserId { get; set; }
  public Users User { get; set; } = null!;

  public string TokenHash { get; set; } = null!;

  public DateTime ExpiresAt { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}