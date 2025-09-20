using RoadGuard.Utils;

namespace RoadGuard.Models.DTO.Report
{
  public class CreateReportRequest
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double RadiusMeters { get; set; }
    public string? Comment { get; set; }
    public int TtlSeconds { get; set; } = TimeUtils.MinutesToMilliseconds(5);
  }
}
