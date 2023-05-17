using CSharpFunctionalExtensions;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.BusinessLogic
{
    public class OAuthUserTokensService : IOAuthUserTokensService
    {
        private readonly IOAuthUserTokensRepository _repository;

        public OAuthUserTokensService(IOAuthUserTokensRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Guid>> CreateOAuthUserToken(OAuthUserToken token)
        {
            if (token is null)
            {
                return Result.Failure<Guid>("Token is null");
            }

            var findToken = await GetTokenById(token.OAuthUserId);
            if (findToken.IsSuccess)
            {
                await _repository.UpdateOAuthUserToken(
                    token with
                    {
                        Id = findToken.Value.Id,
                        OAuthUserId = findToken.Value.OAuthUserId
                    });

                return findToken.Value.OAuthUserId;
            }

            var result = await _repository.CreateOAuthUserToken(token);
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }

            return result.Value;
        }

        public async Task<Result<OAuthUserToken>> GetTokenById(Guid id)
        {
            var token = await _repository.GetToken(id);
            if (token.IsFailure)
            {
                return Result.Failure<OAuthUserToken>(token.Error);
            }

            return token.Value;
        }

        public async Task<OAuthUserToken[]> GetTokens()
        {
            return await _repository.GetTokens();
        }

        public async Task<Result<bool>> UpdateOAuthUserToken(OAuthUserToken token)
        {
            if (token is null)
            {
                return Result.Failure<bool>("Token is null");
            }

            var result = await _repository.UpdateOAuthUserToken(token);
            if (result.IsFailure)
            {
                return Result.Failure<bool>(result.Error);
            }

            return result.Value;
        }
    }
}
