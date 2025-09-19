using Microsoft.Extensions.Logging;
using RoadGuard.Models.DTO.Rating;
using RoadGuard.Models.DTO.User;
using RoadGuard.Models.Entities;
using RoadGuard.Repositories;

namespace RoadGuard.Services
{
  public class RatingService
  {
    private readonly DriverRatingRepository _ratingRepository;
    private readonly UserRepository _userRepository;
    private readonly ILogger<RatingService> _logger;

    public RatingService(
        DriverRatingRepository ratingRepository,
        UserRepository userRepository,
        ILogger<RatingService> logger )
    {
      _ratingRepository = ratingRepository;
      _userRepository = userRepository;
      _logger = logger;
    }

    public async Task<List<DriverRating>> GetRatingsAsync( Guid userId )
    {
      var user = await _userRepository.GetByIdAsync( userId ).ConfigureAwait( false );
      if (user == null) throw new Exception( "User not found" );

      return await _ratingRepository.GetRatingsForUserAsync( userId ).ConfigureAwait( false );
    }

    public async Task RateUserAsync( Guid userId, RateDriverRequest request )
    {
      if (request.Value != 1 && request.Value != -1)
        throw new ArgumentException( "Rating value must be 1 or -1" );

      var fromUser = await _userRepository.GetByIdAsync( request.FromUserId ).ConfigureAwait( false );
      var toUser = await _userRepository.GetByIdAsync( userId ).ConfigureAwait( false );

      if (fromUser == null || toUser == null)
        throw new Exception( "User not found" );

      var existingRating = await _ratingRepository.GetRatingByUsersAsync( request.FromUserId, userId ).ConfigureAwait( false );

      if (existingRating != null)
      {
        existingRating.Value = request.Value;
      }
      else
      {
        existingRating = new DriverRating
        {
          Id = Guid.NewGuid(),
          FromUserId = fromUser.Id,
          ToUserId = toUser.Id,
          Value = request.Value,
          CreatedAt = DateTime.UtcNow,
          FromUser = fromUser,
          ToUser = toUser
        };
        await _ratingRepository.AddRatingAsync( existingRating ).ConfigureAwait( false );
      }

      toUser.ReputationScore = CalculateNewRating( toUser, existingRating );

      await _userRepository.UpdateAsync( toUser ).ConfigureAwait( false );
    }

    private double CalculateWeight( User fromUser, User toUser )
    {
      double reputationFactor = Math.Min( 1, fromUser.ReputationScore / 5.0 );

      double daysInSystem = (DateTime.UtcNow - fromUser.CreatedAt).TotalDays;
      double experienceFactor = Math.Min( 1, daysInSystem / 30.0 );

      int ratingsCount = toUser.RatingsReceived?.Count ?? 0;
      double stabilityFactor = 1.0 / Math.Max( 1, ratingsCount );

      double weight = (0.5 + 0.5 * reputationFactor * experienceFactor) * stabilityFactor;

      return weight;
    }


    private double CalculateNewRating( User toUser, DriverRating newRating )
    {
      var allRatings = toUser.RatingsReceived.ToList();

      double weight = CalculateWeight( newRating.FromUser, toUser );

      double delta = weight * newRating.Value;

      double currentRating = toUser.ReputationScore;

      double newRatingValue = Math.Max( 0, Math.Min( 5, currentRating + delta ) );

      return newRatingValue;
    }
  }
}
