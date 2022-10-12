using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<int> AddCard(CardGuessingStatistics cardStatistics);

        Task<CardGuessingStatistics[]> GetCardGuessingStatistics(Guid? userId);
    }
}
