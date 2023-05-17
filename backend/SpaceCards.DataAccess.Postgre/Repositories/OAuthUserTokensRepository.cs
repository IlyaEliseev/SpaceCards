using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SpaceCards.DataAccess.Postgre.Entites;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Repositories
{
    public class OAuthUserTokensRepository : IOAuthUserTokensRepository
    {
        private readonly SpaceCardsDbContext _context;
        private readonly IMapper _mapper;

        public OAuthUserTokensRepository(SpaceCardsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> CreateOAuthUserToken(OAuthUserToken token)
        {
            if (token is null)
            {
                return Result.Failure<Guid>($"{nameof(token)} is null");
            }

            var entityUserToken = _mapper.Map<OAuthUserToken, OAuthUserTokenEntity>(token);
            await _context.OAuthUserTokens.AddAsync(entityUserToken);
            await _context.SaveChangesAsync();

            return entityUserToken.OAuthUserId;
        }

        public async Task<Result<OAuthUserToken>> GetToken(Guid id)
        {
            var entityUserToken = await _context.OAuthUserTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OAuthUserId == id);

            if (entityUserToken is null)
            {
                return Result.Failure<OAuthUserToken>($"Token with user id: {id} not found");
            }

            return _mapper.Map<OAuthUserTokenEntity, OAuthUserToken>(entityUserToken);
        }

        public async Task<OAuthUserToken[]> GetTokens()
        {
            var entityUserToken = _context.OAuthUserTokens.AsNoTracking().ToArray();
            return _mapper.Map<OAuthUserTokenEntity[], OAuthUserToken[]>(entityUserToken);
        }

        public async Task<Result<bool>> UpdateOAuthUserToken(OAuthUserToken token)
        {
            if (token is null)
            {
                return Result.Failure<bool>($"{nameof(token)} is null");
            }

            var entityUserToken = _mapper.Map<OAuthUserToken, OAuthUserTokenEntity>(token);
            _context.OAuthUserTokens.Update(entityUserToken);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
