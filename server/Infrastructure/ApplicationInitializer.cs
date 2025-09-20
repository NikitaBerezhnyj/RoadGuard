using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadGuard.Data;

namespace RoadGuard.Infrastructure
{
  public class ApplicationInitializer
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApplicationInitializer> _logger;

    public ApplicationInitializer(
        IServiceProvider serviceProvider,
        ILogger<ApplicationInitializer> logger)
    {
      _serviceProvider = serviceProvider;
      _logger = logger;
    }

    public async Task InitializeDatabaseAsync()
    {
      using var scope = _serviceProvider.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

      _logger.LogInformation("🔍 Checking database connection...");

      try
      {
        if (!await dbContext.Database.CanConnectAsync().ConfigureAwait(false))
        {
          _logger.LogError("❌ Cannot connect to database. Check connection string.");
          throw new InvalidOperationException("Database connection failed");
        }
        _logger.LogInformation("✅ Database connection successful");

        var created = await dbContext.Database.EnsureCreatedAsync().ConfigureAwait(false);

        if (created)
        {
          _logger.LogInformation("✅ Database created successfully using EnsureCreated");
        }
        else
        {
          _logger.LogInformation("📋 Database exists. Checking migrations...");

          var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync().ConfigureAwait(false);

          if (pendingMigrations.Any())
          {
            _logger.LogInformation("📦 Applying {Count} pending migrations...", pendingMigrations.Count());
            await dbContext.Database.MigrateAsync().ConfigureAwait(false);
            _logger.LogInformation("✅ Migrations applied successfully");
          }
          else
          {
            _logger.LogInformation("✅ Database is up to date");
          }
        }

        _logger.LogInformation("✅ Database initialization completed successfully");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "❌ Database initialization failed: {Message}", ex.Message);
        throw;
      }
    }
  }
}