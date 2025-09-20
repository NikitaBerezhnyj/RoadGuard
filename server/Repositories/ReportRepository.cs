using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadGuard.Data;
using RoadGuard.Models.Entities;

namespace RoadGuard.Repositories
{
  public class ReportRepository
  {
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ReportRepository> _logger;

    public ReportRepository(AppDbContext dbContext, ILogger<ReportRepository> logger)
    {
      _dbContext = dbContext;
      _logger = logger;
    }

    public async Task<Report?> GetByIdAsync(Guid id)
    {
      try
      {
        return await _dbContext.Reports
          .FirstOrDefaultAsync(r => r.Id == id)
          .ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error getting report {Id}", id);
        throw;
      }
    }

    public async Task<List<Report>> GetActiveReportsAsync()
    {
      try
      {
        var now = DateTime.UtcNow;
        return await _dbContext.Reports
          .Where(r => r.ExpiresAt > now)
          .ToListAsync()
          .ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error querying active reports");
        throw;
      }
    }

    public async Task AddAsync(Report report)
    {
      try
      {
        await _dbContext.Reports.AddAsync(report).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error adding report");
        throw;
      }
    }

    public async Task DeleteAsync(Report report)
    {
      try
      {
        _dbContext.Reports.Remove(report);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error deleting report {Id}", report.Id);
        throw;
      }
    }
  }
}
