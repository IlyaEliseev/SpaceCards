namespace SpaceCards.Domain
{
    public record Card
    {
        public const int MAX_NAME_FRONTSIDE = 200;

        public const int MAX_NAME_BACKSIDE = 200;

        private Card(int id, string frontSide, string backSide, int? groupId)
        {
            Id = id;
            FrontSide = frontSide;
            BackSide = backSide;
            GroupId = groupId;
        }

        public int Id { get; init; }

        public string FrontSide { get; }

        public string BackSide { get; }

        public int? GroupId { get; init; }

        public static (Card? Result, string[] Errors) Create(string frontSide, string backSide)
        {
            var errors = new List<string>();
            var errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(frontSide))
            {
                errorMessage = $"'{nameof(frontSide)}' cannot be null or whitespace.";
                errors.Add(errorMessage);
            }

            if (string.IsNullOrWhiteSpace(backSide))
            {
                errorMessage = $"'{nameof(backSide)}' cannot be null or whitespace.";
                errors.Add(errorMessage);
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var card = new Card(0, frontSide, backSide, null);

            return (card, Array.Empty<string>());
        }
    }
}
