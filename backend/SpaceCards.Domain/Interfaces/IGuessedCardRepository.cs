using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IGuessedCardRepository
    {
        Task<int> AddGuessedCard(Card card);
    }
}
