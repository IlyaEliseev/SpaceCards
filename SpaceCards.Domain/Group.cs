namespace SpaceCards.Domain
{
    public record Group
    {
        public const int MAX_NAME_LENGTH = 200;

        private Group(int id, string name, Card[] cards)
        {
            Id = id;
            Name = name;
            Cards = cards;
        }

        public int Id { get; init; }

        public string Name { get; }

        public Card[] Cards { get; init; }

        public static (Group? Result, string[] Errors) Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, new[] { $"'{nameof(name)}' cannot be null or whitespace." });
            }

            var group = new Group(0, name, Array.Empty<Card>());
            return (group, Array.Empty<string>());
        }
    }
}
