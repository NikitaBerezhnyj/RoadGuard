namespace RoadGuard.Models.DTO.Rating
{
  public class RatingRequestDto
  {
    public Guid ToUserId { get; set; }
    public int Value { get; set; }
  }
}
