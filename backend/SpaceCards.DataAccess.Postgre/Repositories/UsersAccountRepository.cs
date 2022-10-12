using AutoMapper;
using CSharpFunctionalExtensions;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Repositories
{
    public class UsersAccountRepository : IUsersAccountRepository
    {
        private readonly IMapper _mapper;
        private readonly SpaceCardsDbContext _dbContext;

        public UsersAccountRepository(IMapper mapper, SpaceCardsDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Result<bool>> CheckPassword(User user, string passwordHash)
        {
            var expectedUser = _dbContext.Users.FirstOrDefault(
                x => x.Email == user.Email &&
                x.PasswordHash == passwordHash);
            if (expectedUser is null)
            {
                return Result.Failure<bool>("password is uncorrected.");
            }

            return true;
        }

        public async Task<Result<bool>> Create(User user)
        {
            var userEntity = _mapper.Map<User, Entites.UserEntity>(user);

            await _dbContext.Users.AddAsync(userEntity);
            _dbContext.SaveChanges();

            return true;
        }

        public async Task<Result<User>> FindUserByEmail(string email)
        {
            var userEntity = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            if (userEntity is null)
            {
                return Result.Failure<User>("user not found.");
            }

            var user = _mapper.Map<Entites.UserEntity, User>(userEntity);

            return user;
        }

        public async Task<Result<User>> UpdateUser(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
