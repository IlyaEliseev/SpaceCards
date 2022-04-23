namespace SpaceCards.Domain
{
    public record Group
    {
        private Group(int id, string name, Card[] cards)
        {
            Id = id;
            Name = name;
            Cards = cards;
        }

        public int Id { get; init; }

        public string Name { get; }

        public Card[] Cards { get; init; }

        public static Group Create(string name)
        {
            var group = new Group(0, name, Array.Empty<Card>());

            return group;
        }
    }
}
