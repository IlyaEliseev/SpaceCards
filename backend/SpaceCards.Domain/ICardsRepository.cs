namespace SpaceCards.Domain
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