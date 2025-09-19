namespace RoadGuard.Models.DTO

{
  public class RegisterRequest
  {
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? CarMake { get; set; }
    public string? CarColor { get; set; }
    public bool IsAnonymous { get; set; }
  }
}