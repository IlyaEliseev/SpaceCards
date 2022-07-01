using System;
using System.Collections.Generic;

namespace SpaceCards.IntegrationTests.MemberData
{
    internal class GuessedCardsDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidCardIdGroupId(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var invalidCardId = -rnd.Next(0, int.MaxValue);
                var invalidGroupId = -rnd.Next(0, int.MaxValue);

                yield return new object[]
                {
                    invalidCardId, invalidGroupId
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvalidCardIdOrGroupId(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var invlalidId = -rnd.Next(0, int.MaxValue);

                yield return new object[]
                {
                    invlalidId
                };
            }
        }
    }
}
