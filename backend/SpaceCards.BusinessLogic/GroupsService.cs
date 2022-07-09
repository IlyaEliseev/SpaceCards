using SpaceCards.Domain;

namespace SpaceCards.BusinessLogic
{
    public class GroupsService : IGroupsService
    {
        private readonly IGroupsRepository _groupRepository;
        private readonly ICardsRepository _cardsRepository;

        public GroupsService(
            IGroupsRepository groupRepository,
            ICardsRepository cardsRepository)
        {
            _groupRepository = groupRepository;
            _cardsRepository = cardsRepository;
        }

        public async Task<(int Result, string[] Errors)> Create(string name, Guid? userId)
        {
            var (group, errors) = Group.Create(name, userId);
            if (errors.Any())
            {
                return (default, errors);
            }

            var groupId = await _groupRepository.Add(group);
            if (groupId is default(int))
            {
                return (default(int), Array.Empty<string>());
            }

            return (groupId, Array.Empty<string>());
        }

        public async Task<Group[]> Get(Guid? userId)
        {
            return await _groupRepository.Get(userId);
        }

        public async Task<(bool Result, string[] Errors)> AddCard(
            int cardId,
            int groupId)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return (false, new[] { $"'{nameof(card)}' not found." });
            }

            var group = await _groupRepository.GetById(groupId);
            if (group is null)
            {
                return (false, new[] { $"'{nameof(group)}' not found." });
            }

            await _groupRepository.AddCard(card.Id, group.Id);

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Result, string[] Errors)> Update(
            int groupId,
            string updatedGroupName)
        {
            var group = await _groupRepository.GetById(groupId);
            if (group is null)
            {
                return (false, new[] { $"'{nameof(group)}' not found." });
            }

            var (updatedGroup, modelErrors) = Group.Create(updatedGroupName, group.UserId);
            if (modelErrors.Any() || updatedGroup is null)
            {
                return (false, modelErrors.ToArray());
            }

            await _groupRepository.Update(updatedGroup with { Id = group.Id });

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Result, string[] Errors)> Delete(int groupId)
        {
            var group = await _groupRepository.GetById(groupId);
            if (group is null)
            {
                return (false, new[] { $"'{nameof(group)}' not found." });
            }

            if (group.Cards.Count() != 0)
            {
                return (false, new[] { $"'{nameof(group)}' is not empty." });
            }

            await _groupRepository.Delete(group.Id);

            return (true, Array.Empty<string>());
        }

        public async Task<(Group? Result, string[] Error)> GetById(int groupId)
        {
            var group = await _groupRepository.GetById(groupId);
            if (group is null)
            {
                return (null, new[] { $"'{nameof(group)}' not found." });
            }

            return (group, Array.Empty<string>());
        }

        public async Task<(Card[]? Result, string[] Errors)> GetRandomCards(int countCards, Guid? userId)
        {
            if (countCards <= default(int))
            {
                return (null, new[] { $"'{nameof(countCards)}' cannot be 0 or less 0." });
            }

            var randomCards = await _groupRepository.GetRandomCards(countCards, userId);

            return (randomCards, Array.Empty<string>());
        }
    }
}
