using CSharpFunctionalExtensions;

namespace SpaceCards.Domain.Interfaces
{
    public interface IGuessedCardsService
    {
        Task<Result<bool>> SaveGuessedCard(int groupId, int cardId);
    }
}
