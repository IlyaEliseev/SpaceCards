using System.Collections.Generic;

namespace SpaceCards.IntegrationTests.MemberData
{
    internal class GroupsDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidGroupId()
        {
            yield return new object[] { 0 };
            yield return new object[] { -10 };
            yield return new object[] { int.MinValue };
        }

        public static IEnumerable<object[]> GenerateSetInvalidCardIdGroupId()
        {
            yield return new object[] { 0, 0 };
            yield return new object[] { -10, -10 };
            yield return new object[] { int.MinValue, int.MinValue };
            yield return new object[] { 0, -10 };
            yield return new object[] { 0, int.MinValue };
            yield return new object[] { -10, 0 };
            yield return new object[] { int.MinValue, 0 };
        }

        public static IEnumerable<object[]> GenerateSetInvalidGroupIdName()
        {
            var invalidNameLength = BaseDataGenerator.GetInvalidNameLength();
            var nameWithInvalidLength = StringFixture.GenerateRandomString(invalidNameLength);
            yield return new object[] { 0, null };
            yield return new object[] { 0, " " };
            yield return new object[] { 0, "       " };
            yield return new object[] { 0, nameWithInvalidLength };
            yield return new object[] { -10, null };
            yield return new object[] { -10, " " };
            yield return new object[] { -10, "       " };
            yield return new object[] { -10, invalidNameLength };
            yield return new object[] { int.MinValue, null };
            yield return new object[] { int.MinValue, " " };
            yield return new object[] { int.MinValue, "       " };
            yield return new object[] { int.MinValue, nameWithInvalidLength };
        }

        public static IEnumerable<object[]> GenerateSetInvalidName()
        {
            var invalidNameLength = BaseDataGenerator.GetInvalidNameLength();
            var nameWithInvalidLength = StringFixture.GenerateRandomString(invalidNameLength);

            yield return new object[] { null };
            yield return new object[] { " " };
            yield return new object[] { "          " };
            yield return new object[] { nameWithInvalidLength };
        }

        public static IEnumerable<object[]> GenerateSetInvalidCountCards()
        {
            yield return new object[] { 0 };
            yield return new object[] { -10 };
            yield return new object[] { int.MinValue };
        }
    }
}
