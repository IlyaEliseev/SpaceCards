﻿namespace SpaceCards.Domain
{
    public interface ICardsRepository
    {
        Task<int> Add(Card card);

        Task<Card[]> Get();

        Task<Card?> GetById(int cardId);

        Task<bool> Delete(int cardId);

        Task Update(Card card);
    }
}