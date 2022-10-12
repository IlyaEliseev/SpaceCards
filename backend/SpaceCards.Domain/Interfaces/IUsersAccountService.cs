using CSharpFunctionalExtensions;
using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IUsersAccountService
    {
        Task<Result<bool>> Create(User user);

        Task<Result<int>> Update(string email, string password);

        Task<Result<bool>> Delete(string email);

        Task<Result<User>> GetByEmail(string email);
    }
}
