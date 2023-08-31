using System.Collections.Generic;

namespace SpaceCards.IntegrationTests.MemberData
{
    internal class GuessedCardsDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidCardIdGroupId()
        {
            yield return new object[] { 0, 0 };
            yield return new object[] { 0, -10 };
            yield return new object[] { 0, int.MinValue };
            yield return new object[] { -10, 0 };
            yield return new object[] { -10, -10 };
            yield return new object[] { -10, int.MinValue };
            yield return new object[] { int.MinValue, 0 };
            yield return new object[] { int.MinValue, -10 };
            yield return new object[] { int.MinValue, int.MinValue };
        }

        public static IEnumerable<object[]> GenerateSetInvalidId()
        {
            yield return new object[] { 0 };
            yield return new object[] { -10 };
            yield return new object[] { int.MinValue };
        }
    }
}
