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
            if (group is null)
            {
                return (default(int), new[] { $"'{nameof(group)}' not found." });
            }

            var groupEntity = _mapper.Map<Domain.Group, Entites.Group>(group);
            await _context.Groups.AddAsync(groupEntity);
            await _context.SaveChangesAsync();

            return (groupEntity.Id, Array.Empty<string>());
        }

        public async Task<Group[]> Get()
        {
            var groups = await _context.Groups
                .AsNoTracking()
                .Include(x => x.Cards)
                .ToArrayAsync();

            return _mapper.Map<Entites.Group[], Domain.Group[]>(groups);
        }

        public async Task<Group?> GetById(int groupId)
        {
            var group = _context.Groups
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == groupId);

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
            var group = _context.Groups
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == groupId);

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddCard(int cardId, int groupId)
        {
            var card = _context.Cards
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == cardId);

            _context.Cards.Update(new Entites.Card
            {
                Id = cardId,
                FrontSide = card.FrontSide,
                BackSide = card.BackSide,
                GroupId = groupId,
            });

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
