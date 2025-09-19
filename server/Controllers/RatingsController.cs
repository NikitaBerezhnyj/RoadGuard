using Microsoft.AspNetCore.Mvc;
using RoadGuard.Models.DTO.Rating;
using RoadGuard.Services;

namespace RoadGuard.Backend.Controllers
{
  [ApiController]
  [Route( "api/users/{userId}/ratings" )]
  public class RatingController : ControllerBase
  {
    private readonly RatingService _ratingService;
    public RatingController( RatingService ratingService )
    {
      _ratingService = ratingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRatings( Guid userId )
    {
      var ratings = await _ratingService.GetRatingsAsync( userId ).ConfigureAwait( false );
      return Ok( ratings );
    }

    [HttpPost]
    public async Task<IActionResult> RateUser( Guid userId, [FromBody] RateDriverRequest request )
    {
      await _ratingService.RateUserAsync( userId, request ).ConfigureAwait( false );
      return Ok();
    }

  }
}
