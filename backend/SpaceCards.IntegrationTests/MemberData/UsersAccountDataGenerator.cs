using AutoFixture;
using System.Collections.Generic;

namespace SpaceCards.IntegrationTests.MemberData
{
    public class UsersAccountDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidEmail(int testCount)
        {
            var fixture = new Fixture();
            for (int i = 0; i < testCount; i++)
            {
                var email = fixture.Create<string>();
                yield return new object[]
                {
                    email,
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvalidPassword(int testCount)
        {
            var fixture = new Fixture();
            for (int i = 0; i < testCount; i++)
            {
                var password = fixture.Create<string>();
                yield return new object[]
                {
                    password,
                };
            }
        }
    }
}
