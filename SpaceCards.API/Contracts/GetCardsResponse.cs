namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for get cards.
    /// </summary>
    public class GetCardsResponse
    {
        public int Id { get; set; }

        public string FrontSide { get; set; }

        public string BackSide { get; set; }
    }
}
