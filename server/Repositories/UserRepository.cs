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

    public async Task<User?> GetByIdAsync( Guid id )
    {
      try
      {
        return await _dbContext.Users
            .Include( u => u.RatingsGiven )
            .Include( u => u.RatingsReceived )
            .FirstOrDefaultAsync( u => u.Id == id )
            .ConfigureAwait( false );
      }
      catch (Exception ex)
      {
        _logger.LogError( ex, "Error querying user by Id: {Id}", id );
        throw;
      }
    }

    public async Task<User?> GetByLoginAsync( string login )
    {
      try
      {
        return await _dbContext.Users
            .FirstOrDefaultAsync( u => u.Login == login )
            .ConfigureAwait( false );
      }
      catch (Exception ex)
      {
        _logger.LogError( ex, "Error querying user by login: {Login}", login );
        throw;
      }
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

    public async Task UpdateAsync( User user )
    {
      try
      {
        _dbContext.Users.Update( user );
        await _dbContext.SaveChangesAsync().ConfigureAwait( false );
      }
      catch (Exception ex)
      {
        _logger.LogError( ex, "Error updating user: {Username}", user.Username );
        throw;
      }
    }

    public async Task DeleteAsync( User user )
    {
      try
      {
        _dbContext.Users.Remove( user );
        await _dbContext.SaveChangesAsync().ConfigureAwait( false );
      }
      catch (Exception ex)
      {
        _logger.LogError( ex, "Error deleting user: {Username}", user.Username );
        throw;
      }
    }
  }
}
