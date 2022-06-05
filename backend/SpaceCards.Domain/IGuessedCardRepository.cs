namespace SpaceCards.Domain
{
    public interface IGuessedCardRepository
    {
        Task<int> AddGuessedCard(Card card);
    }
}
