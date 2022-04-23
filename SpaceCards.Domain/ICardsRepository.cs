namespace SpaceCards.Domain
{
    public interface ICardsRepository
    {
        Task<int> Add(Card card);

        Task<Card[]> Get();

        Task<(Card? Card, string[] Errors)> GetById(int cardId);

        Task Delete(int cardId);

        Task Update(Card card);
    }
}