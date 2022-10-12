using Microsoft.AspNetCore.Identity;
using SpaceCards.DataAccess.Postgre.Entites;

namespace SpaceCards.BusinessLogic
{
    public class SecurityService : PasswordHasher<UserEntity>
    {
        public string GenerateHashPassword(UserEntity user, string password)
        {
            var passwordHash = HashPassword(user, password);
            return passwordHash;
        }

        public bool VerifyPassword(UserEntity user, string password)
        {
            var result = VerifyHashedPassword(user, user.PasswordHash, password).ToString();
            if (result.Equals("Success"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
