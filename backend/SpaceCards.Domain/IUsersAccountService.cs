using CSharpFunctionalExtensions;

namespace SpaceCards.Domain
{
    public interface IUsersAccountService
    {
        Task<Result<bool>> Create(User user);

        Task<Result<int>> Update(string email, string password);

        Task<Result<bool>> Delete(string email);

        Task<Result<User>> FindUserByEmail(string email);
    }
}
