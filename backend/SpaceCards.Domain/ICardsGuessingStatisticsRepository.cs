namespace SpaceCards.Domain
{
    public interface ICardsGuessingStatisticsRepository
    {
        Task<int> AddCard(Domain.CardGuessingStatistics cardStatistics);

        Task<Domain.CardGuessingStatistics[]> GetCardGuessingStatistics(Guid? userId);
    }
}
