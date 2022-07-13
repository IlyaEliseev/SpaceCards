using System;
using System.Collections.Generic;

namespace SpaceCards.IntegrationTests.MemberData
{
    internal class CardGuessingStatisticsDataGenerator : BaseDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetinvalidCardIdSuccess(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var invalidCardId = -rnd.Next(0, int.MaxValue);
                var invalidSuccess = MakeInvalidSuccess();

                yield return new object[]
                {
                    invalidCardId, invalidSuccess
                };
            }
        }
    }
}
