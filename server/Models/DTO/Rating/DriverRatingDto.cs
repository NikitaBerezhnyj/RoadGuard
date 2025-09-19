namespace RoadGuard.Models.DTO.Rating
{
  public class DriverRatingDto
  {
    public Guid FromUserId { get; set; }
    public string FromUsername { get; set; } = "";
    public Guid ToUserId { get; set; }
    public int Value { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}