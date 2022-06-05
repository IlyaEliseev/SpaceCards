namespace SpaceCards.Domain
{
    public interface IGuessedCardsService
    {
        Task<(bool Result, string[] Errors)> TakeGuessedCard(int groupId, int cardId);
    }
}
