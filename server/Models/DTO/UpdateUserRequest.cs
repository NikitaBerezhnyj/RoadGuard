namespace RoadGuard.Models.DTO
{
    public class UpdateUserRequest
    {
        public string? CarMake { get; set; }
        public string? CarColor { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
