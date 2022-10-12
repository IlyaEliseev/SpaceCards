using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IGroupsService
    {
        Task<(int Result, string[] Errors)> Create(string name, Guid? userId);

        Task<(bool Result, string[] Errors)> Delete(int groupId);

        Task<Group[]> Get(Guid? userId);

        Task<(bool Result, string[] Errors)> Update(int groupId, string groupUdateName);

        Task<(bool Result, string[] Errors)> AddCard(int cardId, int groupId);

        Task<(Group? Result, string[] Error)> GetById(int groupId);

        Task<(Card[]? Result, string[] Errors)> GetRandomCards(int countCards, Guid? userId);
    }
}