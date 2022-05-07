using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpaceCards.Domain;

namespace SpaceCards.DataAccess.Postgre
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly SpaceCardsDbContext _context;
        private readonly IMapper _mapper;

        public GroupsRepository(SpaceCardsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(int Result, string[] Errors)> Add(Group group)
        {
            var groupEntity = _mapper.Map<Domain.Group, Entites.Group>(group);
            await _context.Groups.AddAsync(groupEntity);
            await _context.SaveChangesAsync();

            return (groupEntity.Id, Array.Empty<string>());
        }

        public async Task<Group[]> Get()
        {
            var groups = await _context.Groups
                .AsNoTracking()
                .ToArrayAsync();

            return _mapper.Map<Entites.Group[], Domain.Group[]>(groups);
        }

        public async Task<Group?> GetById(int groupId)
        {
            var group = await _context.Groups
                .AsNoTracking()
                .Include(x => x.Cards)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            return _mapper.Map<Entites.Group, Domain.Group>(group);
        }

        public async Task Update(Group group)
        {
            var groupEntity = _mapper.Map<Domain.Group, Entites.Group>(group);
            _context.Groups.Update(groupEntity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int groupId)
        {
            var group = await _context.Groups
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == groupId);

            _context.Groups.Remove(new Entites.Group { Id = group.Id });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddCard(int cardId, int groupId)
        {
            var card = await _context.Cards
                .FirstOrDefaultAsync(x => x.Id == cardId);

            card.GroupId = groupId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Group?> GetByIdWithCards(int groupId)
        {
            var group = await _context.Groups
                .AsNoTracking()
                .Include(x => x.Cards)
                .Where(x => x.Cards.Count != 0)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            return _mapper.Map<Entites.Group, Domain.Group>(group);
        }
    }
}
