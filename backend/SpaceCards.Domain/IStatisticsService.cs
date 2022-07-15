namespace SpaceCards.Domain
{
    public interface IStatisticsService
    {
        Task<(bool Result, string[] Errors)> CollectCardStatistics(
            int cardId,
            int successValue,
            Guid userId);

        Task<Domain.CardGuessingStatistics[]> GetGuessingCardStatistics(Guid userId);
    }
}
