namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for get card guessing statistics.
    /// </summary>
    public class GetCardGusessingStatisticsResponse
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int Success { get; set; }
    }
}
