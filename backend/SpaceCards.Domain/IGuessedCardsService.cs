namespace SpaceCards.Domain
{
    public interface IGuessedCardsService
    {
        Task<(bool Result, string[] Errors)> SaveGuessedCard(int groupId, int cardId);
    }
}
