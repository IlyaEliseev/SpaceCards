using SpaceCards.Domain;

namespace SpaceCards.BusinessLogic
{
    public class CardsGuessingStatisticsService : ICardsGuessingStatisticsService
    {
        private readonly ICardsRepository _cardsRepository;
        private readonly ICardsGuessingStatisticsRepository _cardsGuessingStatisticsRepository;

        public CardsGuessingStatisticsService(
            ICardsRepository cardsRepository,
            ICardsGuessingStatisticsRepository cardsGuessingStatisticsRepository)
        {
            _cardsRepository = cardsRepository;
            _cardsGuessingStatisticsRepository = cardsGuessingStatisticsRepository;
        }

        public async Task<(bool Result, string[] Errors)> CollectCardStatistics(
            int cardId,
            int successValue,
            Guid? userId)
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

        public async Task<CardGuessingStatistics[]> GetGuessingCardStatistics(Guid? userId)
        {
            var cardGuessingStatistics = await _cardsGuessingStatisticsRepository
                .GetCardGuessingStatistics(userId);

            return cardGuessingStatistics;
        }
    }
}
