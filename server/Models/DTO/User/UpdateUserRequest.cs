namespace RoadGuard.Models.DTO.User
{
  public class UpdateUserRequest
  {
    public string? CarMake { get; set; }
    public string? CarColor { get; set; }
    public bool IsAnonymous { get; set; }
  }
}
