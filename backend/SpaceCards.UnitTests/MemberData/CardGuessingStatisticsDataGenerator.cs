using System.Collections.Generic;

namespace SpaceCards.UnitTests.MemberData
{
    internal class CardGuessingStatisticsDataGenerator : BaseDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidCardIdSuccess()
        {
            yield return new object[] { 0, -1 };
            yield return new object[] { 0, 2 };
            yield return new object[] { -1, -1 };
            yield return new object[] { -1, 2 };
        }
    }
}
