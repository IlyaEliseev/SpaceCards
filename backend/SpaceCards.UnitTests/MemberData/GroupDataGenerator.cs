using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.UnitTests.MemberData
{
    internal class GroupDataGenerator
    {
        private static StringFixture _stringFixture = new StringFixture();

        private static StringFixture StringFixture
        {
            get { return _stringFixture; }
        }

        public static IEnumerable<object[]> GenerateSetInvalidName(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var length = rnd.Next(
                    Group.MAX_NAME_LENGTH + 1,
                    Group.MAX_NAME_LENGTH + 5);

                var invalidData = Enumerable.Range(0, 5)
                   .Select(x => StringFixture.GenerateRandomString(length))
                   .ToArray();

                var invalidName = BaseDataGenerator.MakeInvalidString(invalidData);

                yield return new object[]
                {
                    invalidName
                };
            }
        }
    }
}
