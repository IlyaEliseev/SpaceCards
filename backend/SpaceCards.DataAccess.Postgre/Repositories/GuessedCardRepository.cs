using AutoMapper;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Repositories
{
    public class GuessedCardRepository : IGuessedCardRepository
    {
        private readonly SpaceCardsDbContext _context;
        private readonly IMapper _mapper;

        public GuessedCardRepository(SpaceCardsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddGuessedCard(Card card)
        {
            await _context.GuessedCards.AddAsync(new Entites.GuessedCardEntity { Id = 0, CardId = card.Id });
            await _context.SaveChangesAsync();

            return card.Id;
        }
    }
}
