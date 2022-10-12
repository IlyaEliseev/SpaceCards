namespace SpaceCards.Domain.Interfaces
{
    public interface IGuessedCardsService
    {
        Task<(bool Result, string[] Errors)> SaveGuessedCard(int groupId, int cardId);
    }
}
