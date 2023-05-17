using CSharpFunctionalExtensions;

namespace SpaceCards.Domain.Model
{
    public record OAuthUserToken
    {
        public const int MAX_ACCESS_TOKEN_LENGTH = 500;
        public const int MAX_REFRESH_TOKEN_LENGTH = 500;

        private OAuthUserToken(
            int id,
            Guid userId,
            string accessToken,
            string refreshToken,
            DateTime expireTime)
        {
            Id = id;
            OAuthUserId = userId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpireTime = expireTime;
        }

        public int Id { get; init; }

        public Guid OAuthUserId { get; init; }

        public string AccessToken { get; }

        public string RefreshToken { get; }

        public DateTime ExpireTime { get; }

        public static Result<OAuthUserToken> Create(
            Guid userId,
            string accessToken,
            string refreshToken,
            DateTime expireTime)
        {
            Result failure = Result.Success();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<OAuthUserToken>($"{nameof(OAuthUserToken)} {nameof(accessToken)} can`t be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(accessToken) && accessToken.Length > MAX_ACCESS_TOKEN_LENGTH)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<OAuthUserToken>(
                        $"{nameof(OAuthUserToken)} {nameof(accessToken)} can`t be more than {MAX_ACCESS_TOKEN_LENGTH} chars"));
            }

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<OAuthUserToken>($"{nameof(OAuthUserToken)} {nameof(refreshToken)} can`t be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(refreshToken) && refreshToken.Length > MAX_REFRESH_TOKEN_LENGTH)
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<OAuthUserToken>(
                        $"{nameof(OAuthUserToken)} {nameof(refreshToken)} can`t be more than {MAX_REFRESH_TOKEN_LENGTH} chars"));
            }

            if (failure.IsFailure)
            {
                return Result.Failure<OAuthUserToken>(failure.Error);
            }

            var oauthUserToken = new OAuthUserToken(0, userId, accessToken, refreshToken, expireTime);
            return oauthUserToken;
        }
    }
}
