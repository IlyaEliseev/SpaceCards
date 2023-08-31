using System;
using System.Collections.Generic;

namespace SpaceCards.IntegrationTests.MemberData
{
    internal class CardGuessingStatisticsDataGenerator : BaseDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetinvalidCardIdSuccess()
        {
            yield return new object[] { 0, 2 };
            yield return new object[] { -10, 2 };
            yield return new object[] { int.MinValue, 2 };
            yield return new object[] { 0, -2 };
            yield return new object[] { -10, -2 };
            yield return new object[] { int.MinValue, -2 };
        }
    }
}
