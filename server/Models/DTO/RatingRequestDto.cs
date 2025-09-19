namespace RoadGuard.Models.DTO
{
  public class RatingRequestDto
  {
    public Guid ToUserId { get; set; }
    public int Value { get; set; } // +1 або -1
  }
}
