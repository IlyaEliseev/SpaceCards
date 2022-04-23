namespace SpaceCards.Domain
{
    public record Card
    {
        private Card(int id, string frontSide, string backSide)
        {
            Id = id;
            FrontSide = frontSide;
            BackSide = backSide;
        }

        public int Id { get; init; }

        public string FrontSide { get; }

        public string BackSide { get; }

        public static Card? Create(string frontSide, string backSide)
        {
            var card = new Card(0, frontSide, backSide);
            return card;
        }
    }
}
