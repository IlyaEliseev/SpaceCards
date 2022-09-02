using SpaceCards.Domain;

namespace SpaceCards.BusinessLogic
{
    public class CardsService : ICardsService
    {
        private readonly ICardsRepository _cardsRepository;

        public CardsService(ICardsRepository cardsRepository)
        {
            _cardsRepository = cardsRepository;
        }

        public async Task<(int Result, string[] Errors)> Create(string frontSide, string backSide, Guid? userId)
        {
            var (card, errors) = Card.Create(frontSide, backSide, userId);

            if (errors.Any())
            {
                return (default(int), errors.ToArray());
            }

            var cardId = await _cardsRepository.Add(card);

            return (cardId, Array.Empty<string>());
        }

        public async Task<Card[]> Get(Guid? userId)
        {
            return await _cardsRepository.Get(userId);
        }

        public async Task<(bool Result, string[] Errors)> Delete(int cardId)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return (false, new[] { $"'{nameof(card)}' not found." });
            }

            await _cardsRepository.Delete(card.Id);

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Result, string[] Errors)> Update(
            int cardId,
            string updatedCardFrontSide,
            string updatedCardBackSide)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return (false, new[] { $"'{nameof(card)}' card not found." });
            }

            var (updatedCard, modelErrors) = Card.Create(
                updatedCardFrontSide,
                updatedCardBackSide,
                card.UserId);

            if (modelErrors.Any() || updatedCard is null)
            {
                return (false, modelErrors.ToArray());
            }

            await _cardsRepository.Update(updatedCard with { Id = card.Id, GroupId = card.GroupId });

            return (true, Array.Empty<string>());
        }
    }
}
