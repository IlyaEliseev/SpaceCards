using CSharpFunctionalExtensions;

namespace SpaceCards.Domain.Model
{
    public record Group
    {
        public const int MAX_NAME_LENGTH = 200;

        private Group(int id, string name, Card[] cards, Guid? userId)
        {
            Id = id;
            Name = name;
            Cards = cards;
            UserId = userId;
        }

        public int Id { get; init; }

        public string Name { get; }

        public Card[] Cards { get; init; }

        public Guid? UserId { get; init; }

        public static Result<Group> Create(string name, Guid? userId)
        {
            Result failure = Result.Success();
            if (string.IsNullOrWhiteSpace(name))
            {
                failure = Result.Failure<Group>($"{nameof(Group)} {nameof(name)} cannot be null or whitespace.");
            }

            if (userId is null)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<Group>(
                        $"{nameof(Group)} {nameof(userId)} cannot be null."));
            }

            if (name is not null && name.Length > MAX_NAME_LENGTH)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<Group>(
                        $"{nameof(Group)} {nameof(name)} more than {MAX_NAME_LENGTH} characters."));
            }

            if (failure.IsFailure)
            {
                return Result.Failure<Group>(failure.Error);
            }

            var group = new Group(0, name, Array.Empty<Card>(), userId);

            return group;
        }
    }
}
