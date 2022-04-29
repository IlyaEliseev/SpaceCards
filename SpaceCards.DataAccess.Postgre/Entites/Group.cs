namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
