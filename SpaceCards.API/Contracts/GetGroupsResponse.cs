using SpaceCards.Domain;

namespace SpaceCards.API.Contracts
{
    public class GetGroupsResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
