using CSharpFunctionalExtensions;
using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IGroupsService
    {
        Task<Result<int>> Create(string name, Guid? userId);

        Task<Result<bool>> Delete(int groupId);

        Task<Group[]> Get(Guid? userId);

        Task<Result<bool>> Update(int groupId, string groupUdateName);

        Task<Result<bool>> AddCard(int cardId, int groupId);

        Task<Result<Group>> GetById(int groupId);

        Task<Result<Card[]?>> GetRandomCards(int countCards, Guid? userId);
    }
}