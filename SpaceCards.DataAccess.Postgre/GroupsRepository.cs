using SpaceCards.Domain;

namespace SpaceCards.DataAccess.Postgre
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly SpaceCardsContext _context;

        public GroupsRepository(SpaceCardsContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Group group)
        {
            var groupId = _context.Groups.Count + 1;
            _context.Groups.Add(group with { Id = groupId });
            return groupId;
        }

        public async Task<Group[]> Get()
        {
            return _context.Groups.ToArray();
        }

        public async Task<(Group? Card, string[] Errors)> GetById(int groupId)
        {
            if (groupId <= default(int))
            {
                return (null, new[] { $"'{nameof(groupId)}'cannot be less 0 or 0." });
            }

            var group = _context.Groups.FirstOrDefault(x => x.Id == groupId);

            return (group, Array.Empty<string>());
        }

        public async Task Update(Group group)
        {
            var findGroup = _context.Groups.FirstOrDefault(x => x.Id == group.Id);
            var index = _context.Groups.IndexOf(findGroup);
            _context.Groups.RemoveAt(index);
            _context.Groups.Insert(index, group);
        }

        public async Task Delete(int groupId)
        {
            var (group, errors) = await GetById(groupId);
            _context.Groups.Remove(group);
        }

        public async Task<bool> AddCard(int cardId, int groupId)
        {
            var card = _context.Cards.FirstOrDefault(x => x.Id == cardId);
            var group = _context.Groups.FirstOrDefault(x => x.Id == groupId);
            var newGroup = group with { Cards = group.Cards.Append(card).ToArray() };

            var index = _context.Groups.IndexOf(group);
            _context.Groups.RemoveAt(index);
            _context.Groups.Insert(index, newGroup);

            return true;
        }
    }
}
