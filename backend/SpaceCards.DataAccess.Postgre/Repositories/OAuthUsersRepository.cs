using AutoMapper;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Microsoft.EntityFrameworkCore;
using SpaceCards.DataAccess.Postgre.Entites;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Repositories
{
    public class OAuthUsersRepository : IOAuthUsersRepository
    {
        private readonly SpaceCardsDbContext _context;
        private readonly IMapper _mapper;

        public OAuthUsersRepository(SpaceCardsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> CreateOAuthUser(OAuthUser user)
        {
            if (user is null)
            {
                return Result.Failure<Guid>($"{nameof(user)} null");
            }

            var entityUser = _mapper.Map<OAuthUser, OAuthUserEntity>(user);
            await _context.OAuthUserUsers.AddAsync(entityUser);
            await _context.SaveChangesAsync();

            return entityUser.Id;
        }

        public async Task<Result<bool>> DeleteOAuthUser(Guid id)
        {
            var entityUser = await _context.OAuthUserUsers.FirstOrDefaultAsync(x => x.Id == id);
            if (entityUser is null)
            {
                return Result.Failure<bool>($"User with id: {id} not found");
            }

            entityUser.DeleteDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<OAuthUser[]> GetOAuthUsers()
        {
            var entityUsers = _context.OAuthUserUsers.AsNoTracking().ToArray();
            return _mapper.Map<OAuthUserEntity[], OAuthUser[]>(entityUsers);
        }

        public async Task<Result<OAuthUser>> GetUserByEmail(string email)
        {
            if (email is null)
            {
                return Result.Failure<OAuthUser>("Email is null");
            }

            var oauthUser = await _context.OAuthUserUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);

            if (oauthUser is null)
            {
                return Result.Failure<OAuthUser>($"{oauthUser} is null");
            }

            return _mapper.Map<OAuthUserEntity, OAuthUser>(oauthUser);
        }

        public async Task<Result<OAuthUser>> GetUserById(Guid id)
        {
            var entityUser = await _context.OAuthUserUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entityUser is null)
            {
                return Result.Failure<OAuthUser>($"User with id: {id} not found");
            }

            return _mapper.Map<OAuthUserEntity, OAuthUser>(entityUser);
        }

        public async Task<Result<bool>> UpdateOAuthUser(OAuthUser user)
        {
            if (user is null)
            {
                return Result.Failure<bool>($"User with id: {user.Id} is null");
            }

            var entityUser = _mapper.Map<OAuthUser, OAuthUserEntity>(user);
            _context.OAuthUserUsers.Update(entityUser);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
