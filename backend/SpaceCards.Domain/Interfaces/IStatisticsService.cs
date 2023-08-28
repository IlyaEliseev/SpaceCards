using CSharpFunctionalExtensions;
using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IStatisticsService
    {
        Task<Result<bool>> CollectCardStatistics(
            int cardId,
            int successValue,
            Guid userId);

        Task<CardGuessingStatistics[]> GetGuessingCardStatistics(Guid userId);
    }
}
