﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.IntegrationTests.MemberData
{
    internal class CardDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidCardId(int testCount)
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

        public static IEnumerable<object[]> GenerateSetInvalidCardIdFrontSideBackSide(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var frontsideLength = BaseDataGenerator.GetInvalidFrontsideLength();
                var backsideLength = BaseDataGenerator.GetInvalidBacksideLength();

                var invalidFrontsideData = Enumerable.Range(0, 5)
                    .Select(x => StringFixture.GenerateRandomString(frontsideLength))
                    .ToArray();

                var invalidBacksideData = Enumerable.Range(0, 5)
                    .Select(x => StringFixture.GenerateRandomString(backsideLength))
                    .ToArray();

                var invalidCardId = -rnd.Next(0, int.MaxValue);

                var invalidFrontSide = BaseDataGenerator.MakeInvalidString(invalidFrontsideData);
                var invalidBackSide = BaseDataGenerator.MakeInvalidString(invalidBacksideData);

                yield return new object[]
                {
                    invalidCardId, invalidFrontSide, invalidBackSide
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvalidFrontSideBackSide(int testCount)
        {
            for (int i = 0; i < testCount; i++)
            {
                var frontsideLength = BaseDataGenerator.GetInvalidFrontsideLength();
                var backsideLength = BaseDataGenerator.GetInvalidBacksideLength();

                var invalidFrontsideData = Enumerable.Range(0, 5)
                    .Select(x => StringFixture.GenerateRandomString(frontsideLength))
                    .ToArray();

                var invalidBacksideData = Enumerable.Range(0, 5)
                    .Select(x => StringFixture.GenerateRandomString(backsideLength))
                    .ToArray();

                var invalidFrontSide = BaseDataGenerator.MakeInvalidString(invalidFrontsideData);
                var invalidBackSide = BaseDataGenerator.MakeInvalidString(invalidBacksideData);

                yield return new object[]
                {
                    invalidFrontSide, invalidBackSide
                };
            }
        }
    }
}
