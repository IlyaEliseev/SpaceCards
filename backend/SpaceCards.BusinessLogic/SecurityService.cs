using Microsoft.AspNetCore.Identity;
using SpaceCards.DataAccess.Postgre.Entites;

namespace SpaceCards.BusinessLogic
{
    public class SecurityService : PasswordHasher<User>
    {
        public string HashPassword1(User user, string password)
        {
            var passwordHash = HashPassword(user, password);
            return passwordHash;
        }

        public bool VerifyPassword(User user, string password)
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
