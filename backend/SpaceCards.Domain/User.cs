using CSharpFunctionalExtensions;

namespace SpaceCards.Domain
{
    public record User
    {
        public const int MAX_EMAIL_LENGTH = 256;

        private User(Guid userId, string email, string passwordHash, DateTime registrationData)
        {
            UserId = userId;
            Email = email;
            PasswordHash = passwordHash;
            RegistrationData = registrationData;
        }

        public Guid UserId { get; }

        public string Email { get; }

        public string PasswordHash { get; }

        public DateTime RegistrationData { get; }

        public static Result<User> Create(
            string email,
            string passwordHash,
            DateTime registrationData)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<User>($"{nameof(email)} is incorrect.");
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return Result.Failure<User>($"{nameof(passwordHash)} is incorrect.");
            }

            if (registrationData == default(DateTime))
            {
                return Result.Failure<User>($"{nameof(registrationData)} is incorrect.");
            }

            var user = new User(Guid.Empty, email, passwordHash, registrationData);
            return user;
        }
    }
}
