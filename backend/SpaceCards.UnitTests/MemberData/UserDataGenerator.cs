using SpaceCards.Domain.Model;
using SpaceCards.IntegrationTests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.UnitTests.MemberData
{
    public class UserDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidEmailPasswordHashDatetimeNickname(
            int testCount)
        {
            var rnd = new Random();
            for (int i = 0; i < testCount; i++)
            {
                var invalidNicknameLength = rnd.Next(
                    User.MAX_NICKNAME_LENGTH + 1,
                    User.MAX_NICKNAME_LENGTH + 5);

                var nickname = Enumerable.Range(0, 5)
                    .Select(x => StringFixture.GenerateRandomString(invalidNicknameLength))
                    .ToArray();

                var invalidNickname = BaseDataGenerator.MakeInvalidString(nickname);
                var invalidEmail = BaseDataGenerator.MakeInvalidString();
                var invalidPassword = BaseDataGenerator.MakeInvalidString();

                yield return new object[]
                {
                    invalidEmail, invalidPassword, invalidNickname
                };
            }
        }
    }
}
