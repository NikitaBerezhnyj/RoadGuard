namespace RoadGuard.Models.DTO
{
    public class RateDriverRequest
    {
        public Guid FromUserId { get; set; }
        public int Value { get; set; } // +1 лайк, -1 дизлайк
    }
}
