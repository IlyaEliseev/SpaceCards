using CSharpFunctionalExtensions;

namespace SpaceCards.Domain.Model
{
    public record Card
    {
        public const int MAX_NAME_FRONTSIDE = 200;

        public const int MAX_NAME_BACKSIDE = 200;

        private Card(int id, string? frontSide, string? backSide, int? groupId, Guid? userId)
        {
            Id = id;
            FrontSide = frontSide;
            BackSide = backSide;
            GroupId = groupId;
            UserId = userId;
        }

        public int Id { get; init; }

        public string FrontSide { get; }

        public string BackSide { get; }

        public int? GroupId { get; init; }

        public Guid? UserId { get; }

        public static Result<Card> Create(string frontSide, string backSide, Guid? userId)
        {
            Result failure = Result.Success();
            if (string.IsNullOrWhiteSpace(frontSide))
            {
                failure = Result.Failure<Card>($"{nameof(Card)} {nameof(frontSide)} cannot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(backSide))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<Card>(
                        $"{nameof(Card)} {nameof(backSide)} cannot be null or whitespace."));
            }

            if (userId is null)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<Card>(
                        $"{nameof(Card)} {nameof(userId)} cannot be null."));
            }

            if (frontSide is not null && frontSide.Length > MAX_NAME_FRONTSIDE)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<Card>(
                        $"{nameof(Card)} {nameof(frontSide)} more than {MAX_NAME_FRONTSIDE} characters."));
            }

            if (backSide is not null && backSide.Length > MAX_NAME_BACKSIDE)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<Card>(
                        $"{nameof(Card)} {nameof(backSide)} more than {MAX_NAME_BACKSIDE} characters."));
            }

            if (failure.IsFailure)
            {
                return Result.Failure<Card>(failure.Error);
            }

            var card = new Card(0, frontSide, backSide, null, userId);

            return card;
        }
    }
}
