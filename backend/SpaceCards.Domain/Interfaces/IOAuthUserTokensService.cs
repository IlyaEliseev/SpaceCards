using CSharpFunctionalExtensions;
using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IOAuthUserTokensService
    {
        Task<Result<OAuthUserToken>> GetTokenById(Guid id);

        Task<OAuthUserToken[]> GetTokens();

        Task<Result<Guid>> CreateOAuthUserToken(OAuthUserToken token);

        Task<Result<bool>> UpdateOAuthUserToken(OAuthUserToken token);
    }
}
