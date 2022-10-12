using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface ICardsService
    {
        Task<(int Result, string[] Errors)> Create(string frontSide, string backSide, Guid? userId);

        Task<Card[]> Get(Guid? userId);

        Task<(bool Result, string[] Errors)> Delete(int cardId);

        Task<(bool Result, string[] Errors)> Update(int cardId, string cardUpdatFrontSide, string cardUpdateBackSide);
    }
}
