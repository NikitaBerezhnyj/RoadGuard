using RoadGuard.Models.DTO.User;

namespace RoadGuard.Models.DTO.Auth
{
  public class AuthResponse
  {
    public string Token { get; set; } = null!;
    public UserProfileDto User { get; set; } = null!;
  }
}