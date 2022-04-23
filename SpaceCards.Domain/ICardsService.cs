namespace SpaceCards.Domain
{
    public interface ICardsService
    {
        Task<(int Result, string[] Errors)> Create(string word, string wordTranslate);

        Task<Card[]> Get();

        Task<(bool Result, string[] Errors)> Delete(int cardId);

        Task<(bool Result, string[] Errors)> Update(int cardId, string cardUpdatFrontSide, string cardUpdateBackSide);
    }
}
