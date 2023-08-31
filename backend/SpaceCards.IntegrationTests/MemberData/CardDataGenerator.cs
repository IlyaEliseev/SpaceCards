using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.IntegrationTests.MemberData
{
    internal class CardDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidCardId()
        {
            yield return new object[] { 0 };
            yield return new object[] { -100 };
            yield return new object[] { int.MinValue };
        }

        public static IEnumerable<object[]> GenerateSetInvalidCardIdFrontSideBackSide()
        {
            var frontsideLength = BaseDataGenerator.GetInvalidFrontsideLength();
            var backsideLength = BaseDataGenerator.GetInvalidBacksideLength();
            var frontsideWithInvalidLength = StringFixture.GenerateRandomString(frontsideLength);
            var backsideWithInvalidLength = StringFixture.GenerateRandomString(backsideLength);

            yield return new object[] { 0, null, null };
            yield return new object[] { 0, " ", " " };
            yield return new object[] { 0, "       ", "       " };
            yield return new object[] { 0, frontsideWithInvalidLength, backsideWithInvalidLength };
            yield return new object[] { 0, null, " " };
            yield return new object[] { 0, null, "       " };
            yield return new object[] { 0, null, backsideWithInvalidLength };
            yield return new object[] { 0, " ", null };
            yield return new object[] { 0, "       ", null };
            yield return new object[] { 0, frontsideWithInvalidLength, null };
            yield return new object[] { int.MinValue, null, null };
            yield return new object[] { int.MinValue, " ", " " };
            yield return new object[] { int.MinValue, "       ", "       " };
            yield return new object[] { int.MinValue, frontsideWithInvalidLength, backsideWithInvalidLength };
            yield return new object[] { int.MinValue, null, " " };
            yield return new object[] { int.MinValue, null, "       " };
            yield return new object[] { int.MinValue, null, backsideWithInvalidLength };
            yield return new object[] { int.MinValue, " ", null };
            yield return new object[] { int.MinValue, "       ", null };
            yield return new object[] { int.MinValue, frontsideWithInvalidLength, null };
        }

        public static IEnumerable<object[]> GenerateSetInvalidFrontSideBackSide()
        {
            var frontsideLength = BaseDataGenerator.GetInvalidFrontsideLength();
            var backsideLength = BaseDataGenerator.GetInvalidBacksideLength();
            var frontsideWithInvalidLength = StringFixture.GenerateRandomString(frontsideLength);
            var backsideWithInvalidLength = StringFixture.GenerateRandomString(backsideLength);

            yield return new object[] { null, null};
            yield return new object[] { null, " " };
            yield return new object[] { null, backsideWithInvalidLength };
            yield return new object[] { " ", null };
            yield return new object[] { " ", " " };
            yield return new object[] { " ", backsideWithInvalidLength };
            yield return new object[] { frontsideWithInvalidLength, null };
            yield return new object[] { frontsideWithInvalidLength, " " };
            yield return new object[] { frontsideWithInvalidLength, backsideWithInvalidLength };
        }
    }
}
