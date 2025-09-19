using Microsoft.Extensions.Logging;
using RoadGuard.Models.DTO;
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
            ILogger<RatingService> logger)
        {
            _ratingRepository = ratingRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<DriverRating>> GetRatingsAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            return await _ratingRepository.GetRatingsForUserAsync(userId);
        }

        public async Task RateUserAsync(Guid userId, RateDriverRequest request)
        {
            if (request.Value != 1 && request.Value != -1)
                throw new ArgumentException("Rating value must be 1 or -1");

            var fromUser = await _userRepository.GetByIdAsync(request.FromUserId);
            var toUser = await _userRepository.GetByIdAsync(userId);

            if (fromUser == null || toUser == null)
                throw new Exception("User not found");

            var existingRating = await _ratingRepository.GetRatingByUsersAsync(request.FromUserId, userId);

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
                await _ratingRepository.AddRatingAsync(existingRating);
            }

            // Оновлюємо рейтинг користувача
            toUser.ReputationScore = CalculateNewRating(toUser, existingRating);

            await _userRepository.UpdateAsync(toUser);
        }

        private double CalculateWeight(User fromUser, User toUser)
        {
            // Вага залежить від репутації користувача, що голосує
            double reputationFactor = Math.Min(1, fromUser.ReputationScore / 5.0);

            // Досвід користувача (час в системі в днях)
            double daysInSystem = (DateTime.UtcNow - fromUser.CreatedAt).TotalDays;
            double experienceFactor = Math.Min(1, daysInSystem / 30.0); // максимум через місяць

            // Вага нового голосу: мінімум 0.5, максимум 1
            return 0.5 + 0.5 * reputationFactor * experienceFactor;
        }

        private double CalculateNewRating(User toUser, DriverRating newRating)
        {
            // Всі отримані оцінки користувача
            var allRatings = toUser.RatingsReceived.ToList();

            // Вага нового голосу
            double weight = CalculateWeight(newRating.FromUser, toUser);

            // Підрахунок загальної зміни: враховуємо суму всіх голосів та вагу нового
            double delta = weight * newRating.Value;

            double currentRating = toUser.ReputationScore;

            // Новий рейтинг з обмеженням [0,5]
            double newRatingValue = Math.Max(0, Math.Min(5, currentRating + delta));

            return newRatingValue;
        }
    }
}
