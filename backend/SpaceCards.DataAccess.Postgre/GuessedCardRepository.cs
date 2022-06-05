using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpaceCards.Domain;

namespace SpaceCards.DataAccess.Postgre
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
            await _context.GuessedCards.AddAsync(new Entites.GuessedCard { Id = 0, CardId = card.Id });
            await _context.SaveChangesAsync();

            return card.Id;
        }
    }
}
