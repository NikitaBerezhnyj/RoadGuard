using RoadGuard.Models.DTO.Report;
using RoadGuard.Models.Entities;
using RoadGuard.Repositories;

namespace RoadGuard.Services
{
  public class ReportService
  {
    private readonly ReportRepository _reportRepository;

    public ReportService(ReportRepository reportRepository)
    {
      _reportRepository = reportRepository;
    }

    public async Task<Report?> GetReportAsync(Guid id)
    {
      return await _reportRepository.GetByIdAsync(id).ConfigureAwait(false);
    }

    public async Task<List<Report>> GetActiveReportsAsync()
    {
      return await _reportRepository.GetActiveReportsAsync().ConfigureAwait(false);
    }

    public async Task<Report> CreateReportAsync(CreateReportRequest request)
    {
      var report = new Report
      {
        Id = Guid.NewGuid(),
        Latitude = request.Latitude,
        Longitude = request.Longitude,
        RadiusMeters = request.RadiusMeters,
        Comment = request.Comment,
        CreatedAt = DateTime.UtcNow,
        ExpiresAt = DateTime.UtcNow.AddSeconds(request.TtlSeconds)
      };

      await _reportRepository.AddAsync(report).ConfigureAwait(false);

      // TODO: ReportCreated → повідомити клієнтів через SignalR

      return report;
    }

    public async Task<bool> DeleteReportAsync(Guid id)
    {
      var report = await _reportRepository.GetByIdAsync(id).ConfigureAwait(false);
      if (report == null) return false;

      await _reportRepository.DeleteAsync(report).ConfigureAwait(false);

      // TODO: ReportDeleted → повідомити клієнтів через SignalR

      return true;
    }
  }
}
