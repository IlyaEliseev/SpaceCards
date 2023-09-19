using SpaceCards.Domain.Model;
using SpaceCards.IntegrationTests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.UnitTests.MemberData
{
    public class UserDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidEmailPasswordHashDatetimeNickname()
        {
            var length = User.MAX_NICKNAME_LENGTH + 1;
            var nameWithInvalidLength = StringFixture.GenerateRandomString(length);
            yield return new object[] { null, null, null };
            yield return new object[] { null, " ", " " };
            yield return new object[] { null, null, " " };
            yield return new object[] { null, " ", null };
            yield return new object[] { " " , null, null };
            yield return new object[] { " ", " ", null };
            yield return new object[] { " ", null, " " };
            yield return new object[] { " ", " ", " " };
            yield return new object[] { nameWithInvalidLength, null, null };
            yield return new object[] { nameWithInvalidLength, " ", " " };
            yield return new object[] { nameWithInvalidLength, " ", null };
            yield return new object[] { nameWithInvalidLength, null, " " };
        }
    }
}