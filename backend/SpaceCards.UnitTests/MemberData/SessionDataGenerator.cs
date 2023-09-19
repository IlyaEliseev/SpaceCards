using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.Tests;
using System;
using System.Collections.Generic;

namespace SpaceCards.UnitTests.MemberData
{
    public class SessionDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidUserIdAccessTokenRefreshToken()
        {
            var userId = Guid.Empty;
            var length = Session.MaxLengthToken + 1;
            var tokenWithInvalidLength = StringFixture.GenerateRandomString(length);

            yield return new object[] { userId, null, null };
            yield return new object[] { userId, " ", " " };
            yield return new object[] { userId, tokenWithInvalidLength, tokenWithInvalidLength };
            yield return new object[] { userId, null, " " };
            yield return new object[] { userId, null, tokenWithInvalidLength };
            yield return new object[] { userId, " ", null };
            yield return new object[] { userId, tokenWithInvalidLength, null };
        }
    }
}