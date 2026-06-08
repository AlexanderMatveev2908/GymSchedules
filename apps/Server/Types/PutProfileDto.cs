using System.ComponentModel.DataAnnotations;

namespace Server.FeaturesNS.UserNS;

public class PutProfileDto
{
  [Required]
  [MinLength(3)]
  public string FirstName { get; set; } = null!;

  [Required]
  [MinLength(3)]
  public string LastName { get; set; } = null!;
}