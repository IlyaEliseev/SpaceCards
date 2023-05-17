using CSharpFunctionalExtensions;
using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IOAuthUsersRepository
    {
        Task<Result<Guid>> CreateOAuthUser(OAuthUser user);

        Task<Result<OAuthUser>> GetUserById(Guid id);

        Task<Result<OAuthUser>> GetUserByEmail(string email);

        Task<OAuthUser[]> GetOAuthUsers();

        Task<Result<bool>> UpdateOAuthUser(OAuthUser user);

        Task<Result<bool>> DeleteOAuthUser(Guid id);
    }
}
