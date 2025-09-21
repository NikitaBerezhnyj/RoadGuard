using Microsoft.AspNetCore.SignalR;
using RoadGuard.Models.DTO.Report;
using RoadGuard.Models.Entities;
using RoadGuard.Repositories;
using RoadGuard.Hubs;

namespace RoadGuard.Services
{
  public class ReportService
  {
    private readonly ReportRepository _reportRepository;
    private readonly IHubContext<RoadGuardHub> _hubContext;

    public ReportService(ReportRepository reportRepository, IHubContext<RoadGuardHub> hubContext)
    {
      _reportRepository = reportRepository;
      _hubContext = hubContext;
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

      await _hubContext.Clients.All.SendAsync("ReportCreated", new
      {
        report.Id,
        report.Latitude,
        report.Longitude,
        report.RadiusMeters,
        report.Comment,
        report.CreatedAt,
        report.ExpiresAt
      });

      return report;
    }

    public async Task<bool> DeleteReportAsync(Guid id)
    {
      var report = await _reportRepository.GetByIdAsync(id).ConfigureAwait(false);
      if (report == null) return false;

      await _reportRepository.DeleteAsync(report).ConfigureAwait(false);

      await _hubContext.Clients.All.SendAsync("ReportExpired", report.Id);

      return true;
    }

    public async Task ExpireReportsAsync(List<Guid> expiredIds)
    {
      foreach (var id in expiredIds)
      {
        await _hubContext.Clients.All.SendAsync("ReportExpired", id);
      }
    }
  }
}
