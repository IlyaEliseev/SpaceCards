using SpaceCards.Domain.Model;
using SpaceCards.IntegrationTests;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpaceCards.UnitTests.MemberData
{
    public class OAuthUserDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetWithInvalidNicknameLength()
        {
            var nicknameWithInvalidLength = StringFixture.GenerateRandomString(OAuthUser.MAX_NICKNAME_LENGTH) + 1;
            var email = "test@gmail.com";
            DateTime? deleteDate = null;

            yield return new object[] { nicknameWithInvalidLength, email, deleteDate };
        }

        public static IEnumerable<object[]> GenerateSetWithInvalidEmailLength()
        {
            var nickname = "test";
            var emailWithInvalidLength = StringFixture.GenerateRandomString(OAuthUser.MAX_EMAIL_LENGTH) + 1;
            DateTime? deleteDate = null;

            yield return new object[] { nickname, emailWithInvalidLength, deleteDate };
        }
    }
}
