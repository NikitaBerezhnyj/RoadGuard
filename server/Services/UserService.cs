using RoadGuard.Models.DTO.User;
using RoadGuard.Models.Entities;
using RoadGuard.Repositories;

namespace RoadGuard.Services
{
  public class UserService
  {
    private readonly UserRepository _userRepository;

    public UserService( UserRepository userRepository )
    {
      _userRepository = userRepository;
    }

    public async Task<User?> GetUserAsync( Guid id )
    {
      return await _userRepository.GetByIdAsync( id ).ConfigureAwait( false );
    }

    public async Task<bool> UpdateUserAsync( Guid id, UpdateUserRequest request )
    {
      var user = await _userRepository.GetByIdAsync( id ).ConfigureAwait( false );
      if (user == null) return false;

      user.CarMake = request.CarMake ?? user.CarMake;
      user.CarColor = request.CarColor ?? user.CarColor;
      user.IsAnonymous = request.IsAnonymous;

      await _userRepository.UpdateAsync( user ).ConfigureAwait( false );
      return true;
    }

    public async Task<bool> DeleteUserAsync( Guid id )
    {
      var user = await _userRepository.GetByIdAsync( id ).ConfigureAwait( false );
      if (user == null) return false;

      await _userRepository.DeleteAsync( user ).ConfigureAwait( false );
      return true;
    }
  }
}
