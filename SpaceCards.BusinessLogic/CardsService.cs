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

        public async Task<(int Result, string[] Errors)> Create(string frontSide, string backSide)
        {
            var errors = new List<string>();
            var errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(frontSide))
            {
                errorMessage = $"'{nameof(frontSide)}' cannot be null or whitespace.";
                errors.Add(errorMessage);
            }

            if (string.IsNullOrWhiteSpace(backSide))
            {
                errorMessage = $"'{nameof(backSide)}' cannot be null or whitespace.";
                errors.Add(errorMessage);
            }

            if (errors.Any())
            {
                return (default(int), errors.ToArray());
            }

            var card = Card.Create(frontSide, backSide);
            var cardId = await _cardsRepository.Add(card);

            return (cardId, Array.Empty<string>());
        }

        public async Task<Card[]> Get()
        {
            var cards = await _cardsRepository.Get();
            return cards;
        }

        public async Task<(bool Result, string[] Errors)> Delete(int cardId)
        {
            if (cardId <= default(int))
            {
                return (false, new[] { $"'{nameof(cardId)}' cannot be less 0 or 0." });
            }

            await _cardsRepository.Delete(cardId);

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Result, string[] Errors)> Update(int cardId, string cardUpdatFrontSide, string cardUpdateBackSide)
        {
            var errors = new List<string>();
            var errorMessage = string.Empty;

            if (cardId <= default(int))
            {
                errorMessage = $"'{nameof(cardId)}' cannot be less 0 or 0.";
                errors.Add(errorMessage);
            }

            if (string.IsNullOrWhiteSpace(cardUpdatFrontSide))
            {
                errorMessage = $"'{nameof(cardUpdatFrontSide)}' cannot be null or whitespace.";
                errors.Add(errorMessage);
            }

            if (string.IsNullOrWhiteSpace(cardUpdateBackSide))
            {
                errorMessage = $"'{nameof(cardUpdateBackSide)}' cannot be null or whitespace.";
                errors.Add(errorMessage);
            }

            var (card, contextErrors) = await _cardsRepository.GetById(cardId);

            if (card == null)
            {
                errorMessage = $"'{nameof(card)}' cannot be null.";
                errors.Add(errorMessage);
            }

            if (contextErrors.Any())
            {
                errors.AddRange(contextErrors);
            }

            if (errors.Any())
            {
                return (false, errors.ToArray());
            }

            var updateCard = Card.Create(cardUpdatFrontSide, cardUpdateBackSide);
            await _cardsRepository.Update(updateCard with { Id = card.Id });

            return (true, Array.Empty<string>());
        }
    }
}
