using Microsoft.AspNetCore.Mvc;
using RoadGuard.Models.DTO.Report;
using RoadGuard.Services;

namespace RoadGuard.Backend.Controllers
{
  [ApiController]
  [Route("api/reports")]
  public class ReportController : ControllerBase
  {
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
      _reportService = reportService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetReport(Guid id)
    {
      var report = await _reportService.GetReportAsync(id).ConfigureAwait(false);
      if (report == null) return NotFound();
      return Ok(report);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveReports()
    {
      var reports = await _reportService.GetActiveReportsAsync().ConfigureAwait(false);
      return Ok(reports);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest request)
    {
      var report = await _reportService.CreateReportAsync(request).ConfigureAwait(false);
      return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteReport(Guid id)
    {
      var deleted = await _reportService.DeleteReportAsync(id).ConfigureAwait(false);
      if (!deleted) return NotFound();
      return NoContent();
    }
  }
}
