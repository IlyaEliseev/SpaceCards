using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.MemberData;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests.Tests
{
    public class OAuthUserTests
    {
        [Theory]
        [InlineData("test", "test@gmail.com", null)]
        public async Task Create_oauth_user_with_valid_parameters(
            string nickname,
            string email,
            DateTime? deleteDate)
        {
            // arrange
            // act
            var externalUser = OAuthUser.Create(
                nickname,
                email,
                deleteDate);

            // assert
            Assert.NotNull(externalUser.Value);
            Assert.False(externalUser.IsFailure);
            Assert.Equal(nickname, externalUser.Value.Nickname);
            Assert.Equal(email, externalUser.Value.Email);
            Assert.Equal(deleteDate, externalUser.Value.DeleteDate);
        }

        [Theory]
        [InlineData(" ", "test@gmail.com", null)]
        [InlineData(null, "test@gmail.com", null)]
        public async Task Create_oauth_user_with_null_or_whitespace_nickname(
            string nickname,
            string email,
            DateTime? deleteDate)
        {
            // arrange
            var errorMessage = $"{nameof(OAuthUser)} {nameof(nickname)} can`t be null or whitespace";

            // act
            var externalUser = OAuthUser.Create(
                nickname,
                email,
                deleteDate);

            // assert
            Assert.Equal(errorMessage, externalUser.Error);
            Assert.True(externalUser.IsFailure);
        }

        [Theory]
        [MemberData(
            nameof(OAuthUserDataGenerator.GenerateSetWithInvalidNicknameLength),
            MemberType = typeof(OAuthUserDataGenerator))]
        public async Task Create_oauth_user_with_invalid_nickname_length(
            string nickname,
            string email,
            DateTime? deleteDate)
        {
            // arrange
            var errorMessage = $"{nameof(OAuthUser)} {nameof(nickname)} can`t be more than {OAuthUser.MAX_NICKNAME_LENGTH} chars";

            // act
            var externalUser = OAuthUser.Create(
                nickname,
                email,
                deleteDate);

            // assert
            Assert.Equal(errorMessage, externalUser.Error);
            Assert.True(externalUser.IsFailure);
        }

        [Theory]
        [InlineData("test", " ", null)]
        [InlineData("test", null, null)]
        public async Task Create_oauth_user_with_null_or_whitespace_email(
            string nickname,
            string email,
            DateTime? deleteDate)
        {
            // arrange
            var errorMessage = $"{nameof(OAuthUser)} {nameof(email)} can`t be null or whitespace";

            // act
            var externalUser = OAuthUser.Create(
                nickname,
                email,
                deleteDate);

            // assert
            Assert.Equal(errorMessage, externalUser.Error);
            Assert.True(externalUser.IsFailure);
        }

        [Theory]
        [MemberData(
            nameof(OAuthUserDataGenerator.GenerateSetWithInvalidEmailLength),
            MemberType = typeof(OAuthUserDataGenerator))]
        public async Task Create_oauth_user_with_invalid_email_length(
            string nickname,
            string email,
            DateTime? deleteDate)
        {
            // arrange
            var errorMessage = $"{nameof(OAuthUser)} {nameof(email)} can`t be more than {OAuthUser.MAX_EMAIL_LENGTH} chars";

            // act
            var externalUser = OAuthUser.Create(
                nickname,
                email,
                deleteDate);

            // assert
            Assert.Equal(errorMessage, externalUser.Error);
            Assert.True(externalUser.IsFailure);
        }
    }
}
