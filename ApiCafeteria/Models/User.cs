using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
  public string Name { get; set; }
  public string CPF  { get; set; }
}