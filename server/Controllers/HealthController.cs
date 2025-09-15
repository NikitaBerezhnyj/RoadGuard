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
                status = "ok",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
