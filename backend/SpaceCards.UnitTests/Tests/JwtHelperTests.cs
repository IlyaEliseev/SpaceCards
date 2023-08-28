using SpaceCards.API;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests.Tests
{
    public class JwtHelperTests
    {
        [Fact]
        public async Task Create_access_token()
        {
            // arrange
            var userId = Guid.NewGuid();
            var nickname = "UserNickname";

            var userInformation = new UserInformation(nickname, userId);

            // act
            var accessToken = JwtHelper.CreateAccessToken(userInformation, new API.Options.JWTSecretOptions
            {
                Secret = "secret23423546464"
            });

            // assert
            var payload = JwtHelper.GetPayloadFromJWTToken(accessToken, new API.Options.JWTSecretOptions
            {
                Secret = "secret23423546464"
            });

            var result = JwtHelper.ParseUserInformation(payload);

            Assert.False(string.IsNullOrWhiteSpace(accessToken));
            Assert.Equal(userId, result.Value.UserId);
            Assert.Equal(nickname, result.Value.Nickname);
        }

        [Fact]
        public async Task Create_refresh_token()
        {
            // arrange
            var userId = Guid.NewGuid();
            var nickname = "UserNickname";

            var userInformation = new UserInformation(nickname, userId);

            // act
            var refreshToken = JwtHelper.CreateAccessToken(userInformation, new API.Options.JWTSecretOptions
            {
                Secret = "secret23423546464"
            });

            // assert
            var payload = JwtHelper.GetPayloadFromJWTToken(refreshToken, new API.Options.JWTSecretOptions
            {
                Secret = "secret23423546464"
            });

            var result = JwtHelper.ParseUserInformation(payload);

            Assert.False(string.IsNullOrWhiteSpace(refreshToken));
            Assert.Equal(userId, result.Value.UserId);
            Assert.Equal(nickname, result.Value.Nickname);
        }

        [Fact]
        public async Task Parse_user_information_from_token()
        {
            // arrange
            var userId = Guid.NewGuid();
            var nickname = "UserNickname";

            var userInformation = new UserInformation(nickname, userId);

            var refreshToken = JwtHelper.CreateRefreshToken(userInformation, new API.Options.JWTSecretOptions
            {
                Secret = "secret23423546464"
            });

            var payload = JwtHelper.GetPayloadFromJWTToken(refreshToken, new API.Options.JWTSecretOptions
            {
                Secret = "secret23423546464"
            });

            // act
            var result = JwtHelper.ParseUserInformation(payload);

            // assert
            Assert.Equal(userId, result.Value.UserId);
            Assert.Equal(nickname, result.Value.Nickname);
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }
    }
}
