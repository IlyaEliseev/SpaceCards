using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IGroupsRepository
    {
        Task<int> Add(Group group);

        Task<bool> Delete(int groupId);

        Task<Group[]> Get(Guid? groupId);

        Task<Group?> GetById(int groupId);

        Task Update(Group group);

        Task<bool> AddCard(int cardId, int groupId);

        Task<Card[]> GetRandomCards(int countCards, Guid? groupId);

        Task<Card?> GetCardFromGroupById(int groupId, int cardId);
    }
}