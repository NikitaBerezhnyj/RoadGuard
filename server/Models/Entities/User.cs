using System;

namespace RoadGuard.Models.Entities
{
  public class User
  {
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public string? CarMake { get; set; }
    public string? CarColor { get; set; }
    public bool IsAnonymous { get; set; }

    public double ReputationScore { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<DriverRating> RatingsGiven { get; set; } = new List<DriverRating>();
    public ICollection<DriverRating> RatingsReceived { get; set; } = new List<DriverRating>();
  }
}
