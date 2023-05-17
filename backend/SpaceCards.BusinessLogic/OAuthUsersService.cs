using CSharpFunctionalExtensions;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.BusinessLogic
{
    public class OAuthUsersService : IOAuthUsersService
    {
        private readonly IOAuthUsersRepository _repository;

        public OAuthUsersService(IOAuthUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Guid>> CreateOAuthUser(OAuthUser user)
        {
            if (user is null)
            {
                return Result.Failure<Guid>("User is null");
            }

            var findUser = await _repository.GetUserByEmail(user.Email);
            if (findUser.IsSuccess)
            {
                await _repository.UpdateOAuthUser(
                    user with
                    {
                        Id = findUser.Value.Id,
                        RegistrationData = findUser.Value.RegistrationData
                    });

                return findUser.Value.Id;
            }

            var oauthUser = await _repository.CreateOAuthUser(user);
            if (oauthUser.IsFailure)
            {
                return Result.Failure<Guid>(oauthUser.Error);
            }

            return oauthUser.Value;
        }

        public async Task<Result<bool>> DeleteOAuthUser(Guid id)
        {
            var result = await _repository.DeleteOAuthUser(id);
            if (result.IsFailure)
            {
                return Result.Failure<bool>(result.Error);
            }

            return result;
        }

        public async Task<OAuthUser[]> GetOAuthUsers()
        {
            return await _repository.GetOAuthUsers();
        }

        public async Task<Result<OAuthUser>> GetUserByEmail(string email)
        {
            if (email is null)
            {
                return Result.Failure<OAuthUser>("Email is null");
            }

            var oauthUser = await _repository.GetUserByEmail(email);
            if (oauthUser.IsFailure)
            {
                return Result.Failure<OAuthUser>(oauthUser.Error);
            }

            return oauthUser.Value;
        }

        public async Task<Result<OAuthUser>> GetUserById(Guid id)
        {
            var oauthUser = await _repository.GetUserById(id);
            if (oauthUser.IsFailure)
            {
                return Result.Failure<OAuthUser>(oauthUser.Error);
            }

            return oauthUser.Value;
        }

        public async Task<Result<bool>> UpdateOAuthUser(OAuthUser user)
        {
            if (user is null)
            {
                return Result.Failure<bool>("User is null");
            }

            var result = await _repository.UpdateOAuthUser(user);
            if (result.IsFailure)
            {
                return Result.Failure<bool>(result.Error);
            }

            return result.Value;
        }
    }
}
