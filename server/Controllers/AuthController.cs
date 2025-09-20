using Microsoft.AspNetCore.Mvc;
using RoadGuard.Models.DTO.Auth;
using RoadGuard.Services;

namespace RoadGuard.Backend.Controllers
{
  [ApiController]
  [Route("api/auth")]
  public class AuthController : ControllerBase
  {
    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    {
      _authService = authService;
    }

    [HttpGet("check-username")]
    public async Task<IActionResult> CheckUsername([FromQuery] string username)
    {
      if (string.IsNullOrWhiteSpace(username))
        return BadRequest("Username is required");

      var exists = await _authService.CheckUsernameExistsAsync(username).ConfigureAwait(false);
      return Ok(new { exists });
    }

    [HttpGet("check-login")]
    public async Task<IActionResult> CheckLogin([FromQuery] string login)
    {
      if (string.IsNullOrWhiteSpace(login))
        return BadRequest("Login is required");

      var exists = await _authService.CheckLoginExistsAsync(login).ConfigureAwait(false);
      return Ok(new { exists });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
      var result = await _authService.RegisterAsync(request).ConfigureAwait(false);
      if (result == null)
        return BadRequest("Username already exists");
      return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      var result = await _authService.LoginAsync(request).ConfigureAwait(false);
      if (result == null)
        return Unauthorized("Invalid username or password");
      return Ok(result);
    }
  }
}
