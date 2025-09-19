using System;

namespace RoadGuard.Models.Entities
{
  public class Report
  {
    public Guid Id { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double RadiusMeters { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
  }
}
