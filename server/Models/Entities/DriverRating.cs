using System;

namespace RoadGuard.Models.Entities
{
  public class DriverRating
  {
    public Guid Id { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public int Value { get; set; } // +1 лайк, -1 дизлайк
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User FromUser { get; set; } = null!;
    public User ToUser { get; set; } = null!;
  }
}
