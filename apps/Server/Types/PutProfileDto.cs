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

  // [RegularExpression(
  //      @"^https?:\/\/res\.cloudinary\.com\/",
  //      ErrorMessage = "Image must be hosted on Cloudinary"
  //  )]
  public string? imgFile { get; set; }
}