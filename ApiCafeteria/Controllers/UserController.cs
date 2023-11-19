using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  private readonly UserManager<User> _userManager;
  private readonly SignInManager<User> _signInManager;
  private readonly TokenService _tokenService;

  public UserController(UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _tokenService = tokenService;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register(RegisterModel model)
  {
    var user = new User { UserName = model.Email, Email = model.Email, Name = model.Name, CPF = model.Cpf };
    var result = await _userManager.CreateAsync(user, model.Password);

    if (result.Succeeded)
    {
      await _userManager.AddToRoleAsync(user, model.Email.Contains("admin") ? "Admin" : "User");
      await _signInManager.SignInAsync(user, isPersistent: false);
      return Ok();
    }

    return BadRequest(result.Errors);
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginModel model)
  {
    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

    if (result.Succeeded)
    {
      var user = await _userManager.FindByEmailAsync(model.Email);
      var token = await _tokenService.GenerateTokenAsync(user);
      return Ok(new { Token = token });
    }

    return Unauthorized();
  }

  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [HttpGet("me")]
  public async Task<ActionResult<User>> GetCurrentUser()
  {
    var userId = User.FindFirstValue(ClaimTypes.Name);
    var user = await _userManager.FindByIdAsync(userId);

    if (user == null)
    return Unauthorized();

    return user;
  }
}
