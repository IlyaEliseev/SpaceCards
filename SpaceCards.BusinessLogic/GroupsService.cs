using SpaceCards.Domain;

namespace SpaceCards.BusinessLogic
{
    public class GroupsService : IGroupsService
    {
        private readonly IGroupsRepository _groupRepository;

        public GroupsService(IGroupsRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<(int Result, string[] Errors)> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (default, new[] { $"'{nameof(name)}' cannot be null or whitespace." });
            }

            var group = Group.Create(name);
            var groupId = await _groupRepository.Add(group);

            return (groupId, Array.Empty<string>());
        }

        public async Task<Group[]> Get()
        {
            return await _groupRepository.Get();
        }

        public async Task<(bool Result, string[] Errors)> AddCard(int cardId, int groupId)
        {
            if (cardId <= default(int))
            {
                return (false, new[] { $"'{nameof(cardId)}' cannot be null or whitespace." });
            }

            if (groupId <= default(int))
            {
                return (false, new[] { $"'{nameof(groupId)}' cannot be null or whitespace." });
            }

            await _groupRepository.AddCard(cardId, groupId);

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Result, string[] Errors)> Update(int groupId, string groupUdateName)
        {
            var errors = new List<string>();
            var errorMessage = string.Empty;

            if (groupId <= default(int))
            {
                errorMessage = $"'{nameof(groupId)}' cannot be less 0 or 0.";
                errors.Add(errorMessage);
            }

            if (string.IsNullOrWhiteSpace(groupUdateName))
            {
                errorMessage = $"'{nameof(groupUdateName)}' cannot be null or whitespace.";
                errors.Add(errorMessage);
            }

            var groups = _groupRepository.Get();
            var (group, contextErrors) = await _groupRepository.GetById(groupId);

            if (group == null)
            {
                errorMessage = $"'{nameof(group)}' cannot be null.";
                errors.Add(errorMessage);
            }

            if (contextErrors.Any())
            {
                errors.AddRange(contextErrors);
            }

            if (errors.Any())
            {
                return (false, errors.ToArray());
            }

            var updatedGroup = Group.Create(groupUdateName);
            await _groupRepository.Update(updatedGroup with { Id = group.Id });

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Result, string[] Errors)> Delete(int groupId)
        {
            var errors = new List<string>();
            var errorMessage = string.Empty;

            if (groupId <= default(int))
            {
                errorMessage = $"'{nameof(groupId)}' cannot be 0.";
                errors.Add(errorMessage);
            }

            var (group, contextErrors) = await _groupRepository.GetById(groupId);

            if (group == null)
            {
                errorMessage = $"Group with this {nameof(groupId)} not found.";
                errors.Add(errorMessage);
            }

            if (contextErrors.Any())
            {
                errors.Concat(contextErrors);
            }

            if (errors.Any())
            {
                return (false, errors.ToArray());
            }

            await _groupRepository.Delete(groupId);

            return (true, Array.Empty<string>());
        }
    }
}
