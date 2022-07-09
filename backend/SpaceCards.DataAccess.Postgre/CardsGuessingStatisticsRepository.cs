using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpaceCards.Domain;

namespace SpaceCards.DataAccess.Postgre
{
    public class CardsGuessingStatisticsRepository : ICardsGuessingStatisticsRepository
    {
        private readonly SpaceCardsDbContext _context;
        private readonly IMapper _mapper;

        public CardsGuessingStatisticsRepository(SpaceCardsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddCard(Domain.CardGuessingStatistics cardStatistics)
        {
            var cardStatisticsEntites = _mapper.Map<Domain.CardGuessingStatistics,
                Entites.CardGuessingStatistics>(cardStatistics);

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

            return _mapper.Map<Entites.CardGuessingStatistics[],
                Domain.CardGuessingStatistics[]>(cardGuessingStatistics);
        }
    }
}
