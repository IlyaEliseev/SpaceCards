namespace SpaceCards.Domain
{
    public interface IStatisticsRepository
    {
        Task<int> AddCard(Domain.CardGuessingStatistics cardStatistics);

        Task<Domain.CardGuessingStatistics[]> GetCardGuessingStatistics(Guid? userId);
    }
}
