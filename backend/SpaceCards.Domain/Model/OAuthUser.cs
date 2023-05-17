using CSharpFunctionalExtensions;

namespace SpaceCards.Domain.Model
{
    public record OAuthUser
    {
        public const int MAX_EMAIL_LENGTH = 300;
        public const int MAX_NICKNAME_LENGTH = 300;

        private OAuthUser(
            Guid id,
            string nickname,
            string email,
            DateTime registrationData,
            DateTime? deleteDate)
        {
            Id = id;
            Nickname = nickname;
            Email = email;
            RegistrationData = registrationData;
            DeleteDate = deleteDate;
        }

        public Guid Id { get; init; }

        public string Nickname { get; }

        public string Email { get; }

        public DateTime RegistrationData { get; init; }

        public DateTime? DeleteDate { get; }

        public static Result<OAuthUser> Create(
            string nickname,
            string email,
            DateTime? deleteDate = null)
        {
            Result failure = Result.Success();
            if (string.IsNullOrWhiteSpace(nickname))
            {
                failure = Result.Failure<OAuthUser>($"{nameof(OAuthUser)} {nameof(nickname)} can`t be null or whitespace");
            }

            if (!string.IsNullOrWhiteSpace(nickname) && nickname.Length > MAX_NICKNAME_LENGTH)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<OAuthUser>(
                        $"{nameof(OAuthUser)} {nameof(nickname)} can`t be more than {MAX_NICKNAME_LENGTH} chars"));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<OAuthUser>($"{nameof(OAuthUser)} {nameof(email)} can`t be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(email) && email.Length > MAX_EMAIL_LENGTH)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<OAuthUser>(
                        $"{nameof(OAuthUser)} {nameof(email)} can`t be more than {MAX_EMAIL_LENGTH} chars"));
            }

            if (failure.IsFailure)
            {
                return Result.Failure<OAuthUser>(failure.Error);
            }

            var externalUser = new OAuthUser(
                Guid.Empty,
                nickname,
                email,
                DateTime.UtcNow,
                deleteDate);

            return externalUser;
        }
    }
}
