using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly SpaceCardsDbContext _context;
        private readonly IMapper _mapper;

        public StatisticsRepository(SpaceCardsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddCard(CardGuessingStatistics cardStatistics)
        {
            var cardStatisticsEntites = _mapper.Map<CardGuessingStatistics,
                Entites.CardGuessingStatisticsEntity>(cardStatistics);

            await _context.AddAsync(cardStatisticsEntites);
            await _context.SaveChangesAsync();

            return cardStatisticsEntites.CardId;
        }

        public Task<CardGuessingStatistics[]> GetCardGuessingStatistics()
        {
            throw new NotImplementedException();
        }

        public async Task<CardGuessingStatistics[]> GetCardGuessingStatistics(Guid? userId)
        {
            var cardGuessingStatistics = await _context.CardsGuessingStatistics
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToArrayAsync();

            return _mapper.Map<Entites.CardGuessingStatisticsEntity[],
                CardGuessingStatistics[]>(cardGuessingStatistics);
        }
    }
}
