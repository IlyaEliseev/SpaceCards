using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.IntegrationTests.MemberData
{
    internal class GroupsDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidGroupId(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                yield return new object[]
                {
                    -rnd.Next(0, int.MaxValue)
                };
            }
        }

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

        public static IEnumerable<object[]> GenerateSetInvalidGroupIdName(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var invalidGroupId = -rnd.Next(0, int.MaxValue);

                var invalidNameLength = BaseDataGenerator.GetInvalidNameLength();

                var invalidNameData = Enumerable.Range(0, 5)
                    .Select(x => StringFixture.GenerateRandomString(invalidNameLength))
                    .ToArray();

                var invalidName = BaseDataGenerator.MakeInvalidString(invalidNameData);

                yield return new object[]
                {
                    invalidGroupId, invalidName
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvalidName(int testCount)
        {
            for (int i = 0; i < testCount; i++)
            {
                var invalidNameLength = BaseDataGenerator.GetInvalidNameLength();

                var invalidNameData = Enumerable.Range(0, 5)
                    .Select(x => StringFixture.GenerateRandomString(invalidNameLength))
                    .ToArray();

                var invalidName = BaseDataGenerator.MakeInvalidString(invalidNameData);

                yield return new object[]
                {
                    invalidName
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvaliCountCards(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var cardCount = -rnd.Next(0, int.MaxValue);

                yield return new object[]
                {
                    cardCount
                };
            }
        }
    }
}
