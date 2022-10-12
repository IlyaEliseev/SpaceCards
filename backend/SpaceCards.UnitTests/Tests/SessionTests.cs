using AutoFixture;
using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.MemberData;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests.Tests
{
    public class SessionTests
    {
        private readonly Fixture _fixture;

        public SessionTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateSession_ShouldReturnNewSession()
        {
            // arrange
            var userId = Guid.NewGuid();
            var accessToken = _fixture.Create<string>();
            var refreshToken = _fixture.Create<string>();

            // act
            var result = Session.Create(userId, accessToken, refreshToken);

            // assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(userId, result.Value.UserId);
            Assert.Equal(accessToken, result.Value.AccessToken);
            Assert.Equal(refreshToken, result.Value.RefreshToken);
        }

        [Theory]
        [MemberData(
            nameof(SessionDataGenerator.GenerateSetInvalidUserIdAccessTokenRefreshToken),
            parameters: 10,
            MemberType = typeof(SessionDataGenerator))]
        public async Task CreateSession_CourseIdAccessTokenRefreshTokenIsInvaid_ShouldReturnErrors(
            Guid userId,
            string accessToken,
            string refreshToken)
        {
            // arrange
            // act
            var result = Session.Create(userId, accessToken, refreshToken);

            // assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
        }
    }
}
