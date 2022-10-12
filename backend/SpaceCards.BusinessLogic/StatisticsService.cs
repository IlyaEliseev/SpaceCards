using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.BusinessLogic
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ICardsRepository _cardsRepository;
        private readonly IStatisticsRepository _cardsGuessingStatisticsRepository;

        public StatisticsService(
            ICardsRepository cardsRepository,
            IStatisticsRepository cardsGuessingStatisticsRepository)
        {
            _cardsRepository = cardsRepository;
            _cardsGuessingStatisticsRepository = cardsGuessingStatisticsRepository;
        }

        public async Task<(bool Result, string[] Errors)> CollectCardStatistics(
            int cardId,
            int successValue,
            Guid userId)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return (false, new[] { $"'{nameof(card)}' not found." });
            }

            var (cardStatistics, errors) = CardGuessingStatistics.Create(
                card.Id,
                successValue,
                userId);

            if (errors.Any())
            {
                return (false, errors.ToArray());
            }

            await _cardsGuessingStatisticsRepository.AddCard(cardStatistics);

            return (true, Array.Empty<string>());
        }

        public async Task<CardGuessingStatistics[]> GetGuessingCardStatistics(Guid userId)
        {
            var cardGuessingStatistics = await _cardsGuessingStatisticsRepository
                .GetCardGuessingStatistics(userId);

            return cardGuessingStatistics;
        }
    }
}
