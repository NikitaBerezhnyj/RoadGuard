using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RoadGuard.Data;
using RoadGuard.Services;
using Microsoft.EntityFrameworkCore;

namespace RoadGuard.Infrastructure
{
    public class PeriodicTasksWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PeriodicTasksWorker> _logger;

        public PeriodicTasksWorker(IServiceProvider serviceProvider, ILogger<PeriodicTasksWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PeriodicTasksWorker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await RunReportCleanupAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("PeriodicTasksWorker stopping");
        }

        private async Task RunReportCleanupAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var reportService = scope.ServiceProvider.GetRequiredService<ReportService>();

                var now = DateTime.UtcNow;
                var expiredReports = await db.Reports
                    .Where(r => r.ExpiresAt <= now)
                    .ToListAsync(stoppingToken);

                if (expiredReports.Any())
                {
                    var ids = expiredReports.Select(r => r.Id).ToList();

                    db.Reports.RemoveRange(expiredReports);
                    await db.SaveChangesAsync(stoppingToken);

                    await reportService.ExpireReportsAsync(ids);

                    _logger.LogInformation("Removed {count} expired reports", expiredReports.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while cleaning up reports");
            }
        }
    }
}
