using Microsoft.AspNetCore.Mvc;

namespace RoadGuard.Backend.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
                status = "200",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
