using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Repositories
{
    public class CardsRepository : ICardsRepository
    {
        private readonly SpaceCardsDbContext _context;
        private readonly IMapper _mapper;

        public CardsRepository(SpaceCardsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Add(Card card)
        {
            var cardEntity = _mapper.Map<Card, Entites.CardEntity>(card);
            await _context.Cards.AddAsync(cardEntity);
            await _context.SaveChangesAsync();

            return cardEntity.Id;
        }

        public async Task<Card[]> Get(Guid? userId)
        {
            var cards = await _context.Cards
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToArrayAsync();

            return _mapper.Map<Entites.CardEntity[], Card[]>(cards);
        }

        public async Task<Card?> GetById(int cardId)
        {
            var card = await _context.Cards
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == cardId);

            return _mapper.Map<Entites.CardEntity, Card>(card);
        }

        public async Task Update(Card card)
        {
            var cardEntity = _mapper.Map<Card, Entites.CardEntity>(card);
            _context.Cards.Update(cardEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(int cardId)
        {
            var card = await _context.Cards
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == cardId);

            if (card is null)
            {
                return false;
            }

            _context.Cards.Remove(new Entites.CardEntity { Id = card.Id, GroupId = card.GroupId });
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
