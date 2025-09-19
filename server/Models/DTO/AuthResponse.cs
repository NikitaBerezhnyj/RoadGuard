namespace RoadGuard.Models.DTO
{
  public class AuthResponse
  {
    public string Token { get; set; } = null!;
    public UserProfileDto User { get; set; } = null!;
  }
}