namespace RoadGuard.Models.DTO.Rating
{
  public class RateDriverRequest
  {
    public Guid FromUserId { get; set; }
    public int Value { get; set; }
  }
}
