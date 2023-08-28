using CSharpFunctionalExtensions;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.BusinessLogic
{
    public class CardsService : ICardsService
    {
        private readonly ICardsRepository _cardsRepository;

        public CardsService(ICardsRepository cardsRepository)
        {
            _cardsRepository = cardsRepository;
        }

        public async Task<Result<int>> Create(string frontSide, string backSide, Guid? userId)
        {
            var card = Card.Create(frontSide, backSide, userId);

            if (card.IsFailure)
            {
                return Result.Failure<int>(card.Error);
            }

            var cardId = await _cardsRepository.Add(card.Value);

            return cardId;
        }

        public async Task<Card[]> Get(Guid? userId)
        {
            return await _cardsRepository.Get(userId);
        }

        public async Task<Result<bool>> Delete(int cardId)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return Result.Failure<bool>($"'{nameof(card)}' not found.");
            }

            await _cardsRepository.Delete(card.Id);

            return true;
        }

        public async Task<Result<bool>> Update(
            int cardId,
            string updatedCardFrontSide,
            string updatedCardBackSide)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return Result.Failure<bool>($"'{nameof(card)}' card not found.");
            }

            var result = Card.Create(
                updatedCardFrontSide,
                updatedCardBackSide,
                card.UserId);

            if (result.IsFailure)
            {
                return Result.Failure<bool>(result.Error);
            }

            await _cardsRepository.Update(result.Value with { Id = card.Id, GroupId = card.GroupId });

            return true;
        }
    }
}
