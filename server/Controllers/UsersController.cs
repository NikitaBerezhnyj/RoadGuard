using Microsoft.AspNetCore.Mvc;
using RoadGuard.Models.DTO.User;
using RoadGuard.Services;

namespace RoadGuard.Backend.Controllers
{
  [ApiController]
  [Route( "api/users" )]
  public class UserController : ControllerBase
  {
    private readonly UserService _userService;
    public UserController( UserService userService )
    {
      _userService = userService;
    }

    [HttpGet( "{id:guid}" )]
    public async Task<IActionResult> GetUser( Guid id )
    {
      var user = await _userService.GetUserAsync( id ).ConfigureAwait( false );
      if (user == null) return NotFound();
      return Ok( user );
    }

    [HttpPut( "{id:guid}" )]
    public async Task<IActionResult> UpdateUser( Guid id, [FromBody] UpdateUserRequest request )
    {
      var updated = await _userService.UpdateUserAsync( id, request ).ConfigureAwait( false );
      if (!updated) return NotFound();
      return NoContent();
    }

    [HttpDelete( "{id:guid}" )]
    public async Task<IActionResult> DeleteUser( Guid id )
    {
      var deleted = await _userService.DeleteUserAsync( id ).ConfigureAwait( false );
      if (!deleted) return NotFound();
      return NoContent();
    }
  }
}
