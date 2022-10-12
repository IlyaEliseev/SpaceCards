namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class GuessedCardEntity
    {
        public int Id { get; set; }

        public int? CardId { get; set; }

        public CardEntity? Card { get; set; }
    }
}
