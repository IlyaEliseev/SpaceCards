using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface ICardsRepository
    {
        Task<int> Add(Card card);

        Task<Card[]> Get(Guid? userId);

        Task<Card?> GetById(int cardId);

        Task<bool> Delete(int cardId);

        Task Update(Card card);
    }
}