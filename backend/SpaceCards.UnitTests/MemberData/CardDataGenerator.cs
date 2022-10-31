using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.UnitTests.MemberData
{
    internal class CardDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidFrontside(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var length = rnd.Next(
                    Card.MAX_NAME_FRONTSIDE + 1,
                    Card.MAX_NAME_FRONTSIDE + 5);

                var invalidData = Enumerable.Range(0, 5)
                    .Select(x => StringFixture.GenerateRandomString(length))
                    .ToArray();

                var invalidString = BaseDataGenerator.MakeInvalidString(invalidData);

                yield return new object[]
                {
                    invalidString
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvalidBackside(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var length = rnd.Next(
                    Card.MAX_NAME_BACKSIDE + 1,
                    Card.MAX_NAME_BACKSIDE + 5);

                var invalidData = Enumerable.Range(0, 5)
                    .Select(x => StringFixture.GenerateRandomString(length))
                    .ToArray();

                var invalidString = BaseDataGenerator.MakeInvalidString(invalidData);

                yield return new object[]
                {
                    invalidString
                };
            }
        }
    }
}
