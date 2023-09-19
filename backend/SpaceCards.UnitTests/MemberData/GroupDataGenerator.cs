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

        public static IEnumerable<object[]> GenerateSetInvalidName()
        {
            var length = Group.MAX_NAME_LENGTH + 1;
            var nameWithInvalidLength = StringFixture.GenerateRandomString(length);

            yield return new object[] { null };
            yield return new object[] { "" };
            yield return new object[] { "    " };
            yield return new object[] { nameWithInvalidLength };
        }
    }
}
