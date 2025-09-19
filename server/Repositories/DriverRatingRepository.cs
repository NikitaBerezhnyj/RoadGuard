using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadGuard.Data;
using RoadGuard.Models.Entities;

namespace RoadGuard.Repositories
{
    public class DriverRatingRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<DriverRatingRepository> _logger;

        public DriverRatingRepository(AppDbContext dbContext, ILogger<DriverRatingRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<DriverRating>> GetRatingsForUserAsync(Guid userId)
        {
            try
            {
                return await _dbContext.DriverRatings
                    .Include(r => r.FromUser)
                    .Where(r => r.ToUserId == userId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ratings for user {UserId}", userId);
                throw;
            }
        }

        public async Task AddRatingAsync(DriverRating rating)
        {
            try
            {
                await _dbContext.DriverRatings.AddAsync(rating).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding rating from {FromUserId} to {ToUserId}", rating.FromUserId, rating.ToUserId);
                throw;
            }
        }

        public async Task<DriverRating?> GetRatingByUsersAsync(Guid fromUserId, Guid toUserId)
        {
            try
            {
                return await _dbContext.DriverRatings
                    .FirstOrDefaultAsync(r => r.FromUserId == fromUserId && r.ToUserId == toUserId)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rating from {FromUserId} to {ToUserId}", fromUserId, toUserId);
                throw;
            }
        }
    }
}
