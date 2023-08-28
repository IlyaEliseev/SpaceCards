using CSharpFunctionalExtensions;

namespace SpaceCards.Domain.Model
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

        public static Result<CardGuessingStatistics?> Create(int cardId, int success, Guid userId)
        {
            Result failure = Result.Success();
            if (cardId <= default(int))
            {
                failure = Result.Failure<CardGuessingStatistics?>($"{nameof(cardId)} cannot be 0 or less 0");
            }

            if (success > 1 || success < 0)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<CardGuessingStatistics?>(
                        $"{nameof(success)} should be 0 or 1"));
            }

            if (failure.IsFailure)
            {
                return Result.Failure<CardGuessingStatistics?>(failure.Error);
            }

            var cardStatistics = new CardGuessingStatistics(0, cardId, success, userId);
            return cardStatistics;
        }
    }
}
