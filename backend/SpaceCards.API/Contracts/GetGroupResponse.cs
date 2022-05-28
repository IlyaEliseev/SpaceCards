using SpaceCards.Domain;

namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for get groups.
    /// </summary>
    public class GetGroupResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
