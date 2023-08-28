using CSharpFunctionalExtensions;
using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface ICardsService
    {
        Task<Result<int>> Create(string frontSide, string backSide, Guid? userId);

        Task<Card[]> Get(Guid? userId);

        Task<Result<bool>> Delete(int cardId);

        Task<Result<bool>> Update(int cardId, string cardUpdatFrontSide, string cardUpdateBackSide);
    }
}
