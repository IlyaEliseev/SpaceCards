using CSharpFunctionalExtensions;
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

        public async Task<Result<bool>> CollectCardStatistics(
            int cardId,
            int successValue,
            Guid userId)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return Result.Failure<bool>($"'{nameof(card)}' not found.");
            }

            var cardStatistics = CardGuessingStatistics.Create(
                card.Id,
                successValue,
                userId);

            if (cardStatistics.IsFailure)
            {
                return Result.Failure<bool>(cardStatistics.Error);
            }

            await _cardsGuessingStatisticsRepository.AddCard(cardStatistics.Value);
            return true;
        }

        public async Task<CardGuessingStatistics[]> GetGuessingCardStatistics(Guid userId)
        {
            var cardGuessingStatistics = await _cardsGuessingStatisticsRepository
                .GetCardGuessingStatistics(userId);

            return cardGuessingStatistics;
        }
    }
}
