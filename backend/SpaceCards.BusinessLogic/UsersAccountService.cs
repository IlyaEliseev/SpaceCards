using CSharpFunctionalExtensions;
using SpaceCards.Domain;

namespace SpaceCards.BusinessLogic
{
    public class UsersAccountService : IUsersAccountService
    {
        private readonly IUsersAccountRepository _usersAccountRepository;

        public UsersAccountService(
            IUsersAccountRepository usersAccountRepository)
        {
            _usersAccountRepository = usersAccountRepository;
        }

        public async Task<Result<bool>> Create(User user)
        {
            if (user is null)
            {
                return Result.Failure<bool>($"{nameof(user)} is not found.");
            }

            var result = await _usersAccountRepository.Create(user);
            if (result.IsFailure)
            {
                return Result.Failure<bool>(result.Error);
            }

            return result.Value;
        }

        public async Task<Result<bool>> Delete(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<User>> FindUserByEmail(string email)
        {
            var user = await _usersAccountRepository.FindUserByEmail(email);
            if (user.IsFailure)
            {
                return Result.Failure<User>("User not found.");
            }

            return user.Value;
        }

        public async Task<Result<int>> Update(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
