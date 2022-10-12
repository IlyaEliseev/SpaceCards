using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IStatisticsService
    {
        Task<(bool Result, string[] Errors)> CollectCardStatistics(
            int cardId,
            int successValue,
            Guid userId);

        Task<CardGuessingStatistics[]> GetGuessingCardStatistics(Guid userId);
    }
}
