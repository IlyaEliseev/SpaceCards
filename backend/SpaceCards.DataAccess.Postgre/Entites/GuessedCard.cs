namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class GuessedCard
    {
        public int Id { get; set; }

        public int? CardId { get; set; }

        public Card? Card { get; set; }
    }
}
