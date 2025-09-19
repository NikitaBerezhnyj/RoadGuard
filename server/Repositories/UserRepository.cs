using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadGuard.Data;
using RoadGuard.Models.Entities;

namespace RoadGuard.Repositories
{
  public class UserRepository
  {
    private readonly AppDbContext _dbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository( AppDbContext dbContext, ILogger<UserRepository> logger )
    {
      _dbContext = dbContext;
      _logger = logger;
    }

    public async Task<User?> GetByUsernameAsync( string username )
    {
      try
      {
        return await _dbContext.Users
            .FirstOrDefaultAsync( u => u.Username == username ).ConfigureAwait( false );
      }
      catch (Exception ex)
      {
        _logger.LogError( ex, "Error querying user by username: {Username}", username );
        throw;
      }
    }

    public async Task AddAsync( User user )
    {
      try
      {
        await _dbContext.Users.AddAsync( user ).ConfigureAwait( false );
        await _dbContext.SaveChangesAsync().ConfigureAwait( false );
      }
      catch (Exception ex)
      {
        _logger.LogError( ex, "Error adding user: {Username}", user.Username );
        throw;
      }
    }
  }
}
