using SpaceCards.Domain;

namespace SpaceCards.DataAccess.Postgre
{
    public class SpaceCardsContext
    {
        public SpaceCardsContext()
        {
            Cards = new List<Card>();
            Groups = new List<Group>();
        }

        public List<Card> Cards { get; set; }

        public List<Group> Groups { get; set; }
    }
}
