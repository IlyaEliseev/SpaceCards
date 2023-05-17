using SpaceCards.Domain.Model;
using SpaceCards.IntegrationTests;
using System;
using System.Collections.Generic;

namespace SpaceCards.UnitTests.MemberData
{
    public class OAuthUserTokenDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetWithNullOrWhitespaceAccessToken()
        {
            var userId = Guid.NewGuid();
            var refreshToken = "testRefreshToken";
            var expireAt = DateTime.UtcNow;

            yield return new object[] { userId, " ", refreshToken, expireAt };
            yield return new object[] { userId, null, refreshToken, expireAt };
        }

        public static IEnumerable<object[]> GenerateSetWithNullOrWhitespaceRefreshToken()
        {
            var userId = Guid.NewGuid();
            var accessToken = "testAccessToken";
            var expireAt = DateTime.UtcNow;

            yield return new object[] { userId, accessToken, " ", expireAt };
            yield return new object[] { userId, accessToken, null, expireAt };
        }

        public static IEnumerable<object[]> GenerateSetWithInvalidLengthAccessToken()
        {
            var userId = Guid.NewGuid();
            var accessToken = StringFixture.GenerateRandomString(OAuthUserToken.MAX_ACCESS_TOKEN_LENGTH) + 1;
            var refreshToken = "testRefreshToken";
            var expireAt = DateTime.UtcNow;

            yield return new object[] { userId, accessToken, refreshToken, expireAt };
        }

        public static IEnumerable<object[]> GenerateSetWithInvalidLengthRefreshToken()
        {
            var userId = Guid.NewGuid();
            var accessToken = "testAccessToken";
            var refreshToken = StringFixture.GenerateRandomString(OAuthUserToken.MAX_REFRESH_TOKEN_LENGTH) + 1;
            var expireAt = DateTime.UtcNow;

            yield return new object[] { userId, accessToken, refreshToken, expireAt };
        }
    }
}
