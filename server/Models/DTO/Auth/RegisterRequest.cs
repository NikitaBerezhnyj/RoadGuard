namespace RoadGuard.Models.DTO.Auth
{
  public class RegisterRequest
  {
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? CarMake { get; set; }
    public string? CarColor { get; set; }
    public bool IsAnonymous { get; set; }
  }
}