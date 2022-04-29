﻿using SpaceCards.Domain;

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
            var (card, errors) = Card.Create(frontSide, backSide);

            if (errors.Any())
            {
                return (default(int), errors.ToArray());
            }

            var cardId = await _cardsRepository.Add(card);

            return (cardId, Array.Empty<string>());
        }

        public async Task<Card[]> Get()
        {
            return await _cardsRepository.Get();
        }

        public async Task<(bool Result, string[] Errors)> Delete(int cardId)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return (false, new[] { $"'{nameof(card)}' not found." });
            }

            await _cardsRepository.Delete(cardId);

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Result, string[] Errors)> Update(
                    int cardId,
                    string cardUpdateFrontSide,
                    string cardUpdateBackSide)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return (false, new[] { $"'{nameof(card)}' card not found." });
            }

            var (updateCard, modelErrors) = Card.Create(cardUpdateFrontSide, cardUpdateBackSide);
            if (modelErrors.Any() || updateCard is null)
            {
                return (false, modelErrors.ToArray());
            }

            await _cardsRepository.Update(updateCard with { Id = card.Id });

            return (true, Array.Empty<string>());
        }
    }
}