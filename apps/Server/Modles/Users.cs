using InvoicesApp.TypesNS.UsersNS;

namespace InvoicesApp.ModelsNS.UsersNS;


public class Users
{
  public int Id { get; set; }
  public string FirstName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;

  public Users()
  {
  }

  public Users(UsersDto dto)
  {
    FirstName = dto.FirstName;
    LastName = dto.LastName;
    Email = dto.Email;
    Password = dto.Password;
  }
}