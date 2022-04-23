using SpaceCards.Domain;

namespace SpaceCards.DataAccess.Postgre
{
    public class CardsRepository : ICardsRepository
    {
        private readonly SpaceCardsContext _context;

        public CardsRepository(SpaceCardsContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Card card)
        {
            var cardId = _context.Cards.Count + 1;
            _context.Cards.Add(card with { Id = cardId });
            return cardId;
        }

        public async Task<Card[]> Get()
        {
            return _context.Cards.ToArray();
        }

        public async Task<(Card? Card, string[] Errors)> GetById(int cardId)
        {
            if (cardId <= default(int))
            {
                return (null, new[] { $"'{nameof(cardId)}'cannot be less 0 or 0." });
            }

            var card = _context.Cards.FirstOrDefault(x => x.Id == cardId);

            return (card, Array.Empty<string>());
        }

        public async Task Update(Card card)
        {
            var findCard = _context.Cards.FirstOrDefault(x => x.Id == card.Id);
            var index = _context.Cards.IndexOf(findCard);
            _context.Cards.RemoveAt(index);
            _context.Cards.Insert(index, card);
        }

        public async Task Delete(int cardId)
        {
            var (card, errors) = await GetById(cardId);
            _context.Cards.Remove(card);
        }
    }
}
