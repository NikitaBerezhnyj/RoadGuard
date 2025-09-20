using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RoadGuard.Models.Entities;
using RoadGuard.Models.DTO.Auth;
using RoadGuard.Models.DTO.User;
using RoadGuard.Repositories;
using System.Security.Claims;

namespace RoadGuard.Services
{
  public class AuthService
  {
    private readonly UserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(UserRepository userRepository, IConfiguration configuration)
    {
      _userRepository = userRepository;
      _configuration = configuration;
    }

    public async Task<bool> CheckUsernameExistsAsync(string username)
    {
      var user = await _userRepository.GetByUsernameAsync(username).ConfigureAwait(false);
      return user != null;
    }

    public async Task<bool> CheckLoginExistsAsync(string login)
    {
      var user = await _userRepository.GetByLoginAsync(login).ConfigureAwait(false);
      return user != null;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
      if (request is null) throw new ArgumentNullException(nameof(request));

      if (await _userRepository.GetByLoginAsync(request.Login).ConfigureAwait(false) != null)
        return null;

      var user = new User
      {
        Login = request.Login,
        Username = request.Username,
        PasswordHash = HashPassword(request.Password),
        CarMake = request.CarMake,
        CarColor = request.CarColor,
        IsAnonymous = request.IsAnonymous
      };

      await _userRepository.AddAsync(user).ConfigureAwait(false);

      var token = GenerateJwtToken(user);

      return new AuthResponse
      {
        Token = token,
        User = new UserProfileDto
        {
          Username = user.Username,
          CarMake = user.CarMake,
          CarColor = user.CarColor,
          IsAnonymous = user.IsAnonymous,
          ReputationScore = user.ReputationScore
        }
      };
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
      var user = await _userRepository.GetByLoginAsync(request.Login).ConfigureAwait(false);
      if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        return null;

      var token = GenerateJwtToken(user);

      return new AuthResponse
      {
        Token = token,
        User = new UserProfileDto
        {
          Username = user.Username,
          CarMake = user.CarMake,
          CarColor = user.CarColor,
          IsAnonymous = user.IsAnonymous,
          ReputationScore = user.ReputationScore
        }
      };
    }

    private static string HashPassword(string password)
    {
      var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
      return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
      return HashPassword(password) == hash;
    }

    private string GenerateJwtToken(User user)
    {
      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim("login", user.Login),
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: _configuration["Jwt:Issuer"],
          audience: _configuration["Jwt:Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddDays(7),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
