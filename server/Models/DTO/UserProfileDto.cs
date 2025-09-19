namespace RoadGuard.Models.DTO
{
  public class UserProfileDto
  {
    public string Username { get; set; } = null!;
    public string? CarMake { get; set; }
    public string? CarColor { get; set; }
    public bool IsAnonymous { get; set; }
    public double ReputationScore { get; set; }
  }
}
