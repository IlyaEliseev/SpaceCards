namespace SpaceCards.Domain
{
    public record CardGuessingStatistics
    {
        private CardGuessingStatistics(int id, int cardId, int success, Guid userId)
        {
            Id = id;
            CardId = cardId;
            Success = success;
            UserId = userId;
        }

        public int Id { get; }

        public int CardId { get; }

        public int Success { get; }

        public Guid UserId { get; }

        public static (CardGuessingStatistics? Result, string[] Errors) Create(int cardId, int success,  Guid userId)
        {
            if (cardId <= default(int))
            {
                return (null, new[] { $"{nameof(cardId)} cannot be 0 or less 0" });
            }

            if (success > 1 || success < 0)
            {
                return (null, new[] { $"{nameof(success)} should be 0 or 1" });
            }

            var cardStatistics = new CardGuessingStatistics(0, cardId, success, userId);

            return (cardStatistics, Array.Empty<string>());
        }
    }
}
