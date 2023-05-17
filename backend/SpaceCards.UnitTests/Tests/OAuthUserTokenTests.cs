using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.MemberData;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests.Tests
{
    public class OAuthUserTokenTests
    {
        [Fact]
        public async Task Create_oauth_user_token_with_valid_parameters()
        {
            // arrange
            var userId = Guid.NewGuid();
            string accessToken = "testAccessToken";
            string refreshToken = "testRefreshToken";
            DateTime expireAt = DateTime.UtcNow;

            // act
            var oauthUserToken = OAuthUserToken.Create(userId, accessToken, refreshToken, expireAt);

            // assert
            Assert.NotNull(oauthUserToken.Value);
            Assert.False(oauthUserToken.IsFailure);
            Assert.Equal(userId, oauthUserToken.Value.OAuthUserId);
            Assert.Equal(accessToken, oauthUserToken.Value.AccessToken);
            Assert.Equal(refreshToken, oauthUserToken.Value.RefreshToken);
        }

        [Theory]
        [MemberData(
            nameof(OAuthUserTokenDataGenerator.GenerateSetWithNullOrWhitespaceAccessToken),
            MemberType = typeof(OAuthUserTokenDataGenerator))]
        public async Task Create_oauth_user_token_with_null_or_whitespace_access_token(
            Guid userId,
            string accessToken,
            string refreshToken,
            DateTime expireAt)
        {
            // arrange
            var errorMessage = $"{nameof(OAuthUserToken)} {nameof(accessToken)} can`t be null or whitespace";

            // act
            var oauthUserToken = OAuthUserToken.Create(userId, accessToken, refreshToken, expireAt);

            // assert
            Assert.Equal(errorMessage, oauthUserToken.Error);
            Assert.True(oauthUserToken.IsFailure);
        }

        [Theory]
        [MemberData(
            nameof(OAuthUserTokenDataGenerator.GenerateSetWithInvalidLengthAccessToken),
            MemberType = typeof(OAuthUserTokenDataGenerator))]
        public async Task Create_oauth_user_token_with_invalid_length_access_token(
            Guid userId,
            string accessToken,
            string refreshToken,
            DateTime expireAt)
        {
            // arrange
            var errorMessage =
                $"{nameof(OAuthUserToken)} {nameof(accessToken)} can`t be more than {OAuthUserToken.MAX_ACCESS_TOKEN_LENGTH} chars";

            // act
            var oauthUserToken = OAuthUserToken.Create(userId, accessToken, refreshToken, expireAt);

            // assert
            Assert.Equal(errorMessage, oauthUserToken.Error);
            Assert.True(oauthUserToken.IsFailure);
        }

        [Theory]
        [MemberData(
            nameof(OAuthUserTokenDataGenerator.GenerateSetWithNullOrWhitespaceRefreshToken),
            MemberType = typeof(OAuthUserTokenDataGenerator))]
        public async Task Create_oauth_user_token_with_with_null_or_whitespace_refresh_token(
            Guid userId,
            string accessToken,
            string refreshToken,
            DateTime expireAt)
        {
            // arrange
            var errorMessage =
                $"{nameof(OAuthUserToken)} {nameof(refreshToken)} can`t be null or whitespace";

            // act
            var oauthUserToken = OAuthUserToken.Create(userId, accessToken, refreshToken, expireAt);

            // assert
            Assert.Equal(errorMessage, oauthUserToken.Error);
            Assert.True(oauthUserToken.IsFailure);
        }

        [Theory]
        [MemberData(
            nameof(OAuthUserTokenDataGenerator.GenerateSetWithInvalidLengthRefreshToken),
            MemberType = typeof(OAuthUserTokenDataGenerator))]
        public async Task Create_oauth_user_token_with_invalid_length_refresh_token(
            Guid userId,
            string accessToken,
            string refreshToken,
            DateTime expireAt)
        {
            // arrange
            var errorMessage =
                $"{nameof(OAuthUserToken)} {nameof(refreshToken)} can`t be more than {OAuthUserToken.MAX_REFRESH_TOKEN_LENGTH} chars";

            // act
            var oauthUserToken = OAuthUserToken.Create(userId, accessToken, refreshToken, expireAt);

            // assert
            Assert.Equal(errorMessage, oauthUserToken.Error);
            Assert.True(oauthUserToken.IsFailure);
        }
    }
}
