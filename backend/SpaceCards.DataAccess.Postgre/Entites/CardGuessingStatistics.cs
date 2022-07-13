namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class CardGuessingStatistics
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int Success { get; set; }

        public Guid? UserId { get; set; }
    }
}
