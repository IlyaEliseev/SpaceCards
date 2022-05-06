namespace SpaceCards.Domain
{
    public interface IGroupsRepository
    {
        Task<(int Result, string[] Errors)> Add(Group group);

        Task Delete(int groupId);

        Task<Group[]> Get();

        Task<Group?> GetById(int groupId);

        Task Update(Group group);

        Task<bool> AddCard(int cardId, int groupId);

        Task<Group?> GetByIdWithCards(int groupId);
    }
}