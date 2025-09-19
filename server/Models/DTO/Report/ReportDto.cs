namespace RoadGuard.Models.DTO.Report
{
  public class ReportDto
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double RadiusMeters { get; set; }
    public string? Comment { get; set; }
  }
}
