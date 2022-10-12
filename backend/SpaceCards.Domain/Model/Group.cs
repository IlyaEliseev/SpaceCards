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

        public static (Group? Result, string[] Errors) Create(string name, Guid? userId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, new[] { $"'{nameof(name)}' cannot be null or whitespace." });
            }

            if (userId is null)
            {
                return (null, new[] { $"{userId} cannot be null." });
            }

            if (name is not null && name.Length > MAX_NAME_LENGTH)
            {
                return (null, new[] { $"'{nameof(name)}' more than {MAX_NAME_LENGTH} characters." });
            }

            var group = new Group(0, name, Array.Empty<Card>(), userId);

            return (group, Array.Empty<string>());
        }
    }
}
