namespace SpaceCards.Domain
{
    public interface IGroupsRepository
    {
        Task<int> Add(Group group);

        Task Delete(int groupId);

        Task<Group[]> Get();

        Task<(Group? Card, string[] Errors)> GetById(int groupId);

        Task Update(Group group);

        Task<bool> AddCard(int cardId, int groupId);
    }
}