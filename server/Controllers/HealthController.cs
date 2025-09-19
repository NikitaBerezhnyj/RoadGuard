using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadGuard.Data;

namespace RoadGuard.Backend.Controllers
{
  [ApiController]
  [Route( "api/health" )]
  public class HealthController : ControllerBase
  {
    private readonly AppDbContext _dbContext;

    public HealthController( AppDbContext dbContext )
    {
      _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
      try
      {
        var canConnect = await _dbContext.Database.CanConnectAsync().ConfigureAwait( false );

        if (canConnect)
        {
          return Ok( new
          {
            status = "OK",
            timestamp = DateTime.UtcNow,
            message = "Success"
          } );
        }
        else
        {
          return StatusCode( 503, new
          {
            status = "ERROR",
            timestamp = DateTime.UtcNow,
            message = "Database is not reachable"
          } );
        }
      }
      catch (Exception ex)
      {
        return StatusCode( 503, new
        {
          status = "ERROR",
          timestamp = DateTime.UtcNow,
          message = $"Database connection failed: {ex.Message}"
        } );
      }
    }
  }
}