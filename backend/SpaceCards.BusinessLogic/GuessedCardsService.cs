﻿using CSharpFunctionalExtensions;
using SpaceCards.Domain.Interfaces;

namespace SpaceCards.BusinessLogic
{
    public class GuessedCardsService : IGuessedCardsService
    {
        private readonly IGroupsRepository _groupRepository;
        private readonly IGuessedCardRepository _guessedCardRepository;

        public GuessedCardsService(
            IGroupsRepository groupRepository,
            IGuessedCardRepository guessedCardRepository)
        {
            _guessedCardRepository = guessedCardRepository;
            _groupRepository = groupRepository;
        }

        public async Task<Result<bool>> SaveGuessedCard(int groupId, int cardId)
        {
            var card = await _groupRepository.GetCardFromGroupById(groupId, cardId);
            if (card is null)
            {
                return Result.Failure<bool>($"'{nameof(card)}' not found.");
            }

            await _guessedCardRepository.AddGuessedCard(card);

            return true;
        }
    }
}
