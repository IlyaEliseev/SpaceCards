using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.UnitTests.MemberData
{
    internal class CardDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidFrontside()
        {
            var length = Card.MAX_NAME_FRONTSIDE + 1;
            var frontSideWithInvalidLength = StringFixture.GenerateRandomString(length);

            yield return new object[] { null };
            yield return new object[] { "" };
            yield return new object[] { "    " };
            yield return new object[] { frontSideWithInvalidLength };
        }

        public static IEnumerable<object[]> GenerateSetInvalidBackside()
        {
            var length = Card.MAX_NAME_BACKSIDE + 1;
            var backSideWithInvalidLength = StringFixture.GenerateRandomString(length);

            yield return new object[] { null };
            yield return new object[] { "" };
            yield return new object[] { "    " };
            yield return new object[] { backSideWithInvalidLength };
        }
    }
}
