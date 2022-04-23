namespace SpaceCards.Domain
{
    public interface IGroupsService
    {
        Task<(int Result, string[] Errors)> Create(string name);

        Task<(bool Result, string[] Errors)> Delete(int groupId);

        Task<Group[]> Get();

        Task<(bool Result, string[] Errors)> Update(int groupId, string groupUdateName);

        Task<(bool Result, string[] Errors)> AddCard(int cardId, int groupId);
    }
}