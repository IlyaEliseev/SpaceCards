namespace SpaceCards.Domain
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

        public static (Card? Result, string[] Errors) Create(string frontSide, string backSide, Guid? userId)
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

            if (userId is null)
            {
                errorMessage = $"{userId} cannot be null.";
                errors.Add(errorMessage);
            }

            if (frontSide is not null && frontSide.Length > MAX_NAME_FRONTSIDE)
            {
                errorMessage = $"'{nameof(frontSide)}' more than {MAX_NAME_FRONTSIDE} characters.";
                errors.Add(errorMessage);
            }

            if (backSide is not null && backSide.Length > MAX_NAME_BACKSIDE)
            {
                errorMessage = $"'{nameof(backSide)}' more than {MAX_NAME_BACKSIDE} characters.";
                errors.Add(errorMessage);
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var card = new Card(0, frontSide, backSide, null, userId);

            return (card, Array.Empty<string>());
        }
    }
}
