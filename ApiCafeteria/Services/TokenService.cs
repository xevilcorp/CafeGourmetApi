using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
  private readonly IConfiguration _configuration;
  private readonly UserManager<User> _userManager;

  public TokenService(IConfiguration configuration, UserManager<User> userManager)
  {
    _configuration = configuration;
    _userManager = userManager;
  }
  public async Task<string> GenerateTokenAsync(User user)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var keyString = _configuration["JwtConfig:Secret"];
    var key = Encoding.ASCII.GetBytes(keyString);
    var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = await GenerateClaimsAsync(user),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = credentials
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  private async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
  {
    var ci = new ClaimsIdentity();
    ci.AddClaim(new Claim(ClaimTypes.Name, user.Id.ToString()));
    ci.AddClaim(new Claim(ClaimTypes.Email, user.Email.ToString()));

    var roles = await _userManager.GetRolesAsync(user);

    foreach (var role in roles)
    {
      ci.AddClaim(new Claim(ClaimTypes.Role, role));
    }
    return ci;
  }
}