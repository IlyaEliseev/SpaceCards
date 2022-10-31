using CSharpFunctionalExtensions;

namespace SpaceCards.Domain.Model
{
    public record User
    {
        public const int MAX_NICKNAME_LENGTH = 50;

        private User(Guid userId, string email, string passwordHash, DateTime registrationData, string nickname)
        {
            UserId = userId;
            Email = email;
            PasswordHash = passwordHash;
            RegistrationData = registrationData;
            Nickname = nickname;
        }

        public Guid UserId { get; }

        public string Email { get; }

        public string Nickname { get; }

        public string PasswordHash { get; }

        public DateTime RegistrationData { get; }

        public static Result<User> Create(
            string email,
            string passwordHash,
            string nickname)
        {

            Result failure = Result.Success();
            if (string.IsNullOrWhiteSpace(email))
            {
                failure = Result.Failure<User>($"{nameof(email)} is incorrect.");
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<User>($"{nameof(passwordHash)} is incorrect."));
            }

            if (string.IsNullOrWhiteSpace(nickname))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<User>($"{nameof(nickname)} is not be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(nickname)
                && nickname.Length > MAX_NICKNAME_LENGTH)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<User>($"{nameof(nickname)} is not be more than {MAX_NICKNAME_LENGTH} chars"));
            }

            if (failure.IsFailure)
            {
                return Result.Failure<User>(failure.Error);
            }

            var user = new User(Guid.Empty, email, passwordHash, DateTime.UtcNow, nickname);
            return user;
        }
    }
}
