using SpaceCards.Domain;
using System;
using System.Collections.Generic;

namespace SpaceCards.UnitTests
{
    internal class CardDataGenerator
    {
        private static StringFixture _stringFixture = new StringFixture();

        private static StringFixture StringFixture
        {
            get { return _stringFixture; }
        }

        public static IEnumerable<object[]> GenerateSetInvalidFrontSideOrBackSide(int testCount)
        {
            for (int i = 0; i < testCount; i++)
            {
                var invlidString = MakeInvalidString();

                yield return new object[]
                {
                    invlidString
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetFronSideWithInvalidLength(int testCount)
        {
            for (int i = 0; i < testCount; i++)
            {
                var length = BaseDataGenerator.Rnd
                    .Next(
                    Card.MAX_NAME_FRONTSIDE + 1,
                    int.MaxValue / 1000);

                var str = StringFixture.GenerateRandomString(length);

                var invStr = MakeInvalidString(str);

                yield return new object[]
                {
                    invStr
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetBackSideWithInvalidLength(int testCount)
        {
            for (int i = 0; i < testCount; i++)
            {
                var length = BaseDataGenerator.Rnd
                    .Next(
                    Card.MAX_NAME_BACKSIDE + 1,
                    int.MaxValue / 1000);

                yield return new object[]
                {
                    StringFixture.GenerateRandomString(length)
                };
            }
        }

        private static List<string> GetInvalidFrontSideOrBackSide(int count)
        {
            var data = new List<string>
            {
                null,
                string.Empty
            };

            for (int i = 0; i < count; i++)
            {
                var whiteSpace = string.Empty.PadLeft(
                    BaseDataGenerator.Rnd.Next(1, 100), ' ');

                if (!data.Contains(whiteSpace))
                {
                    data.Add(whiteSpace);
                }
            }

            return data;
        }

        private static string MakeInvalidString(params string[] invalidData)
        {
            var data = new List<string>
            {
                null,
                string.Empty
            };

            data.AddRange(invalidData ?? Array.Empty<string>());

            var whiteSpace = string.Empty.PadLeft(
                BaseDataGenerator.Rnd.Next(1, 100), ' ');

            data.Add(whiteSpace);

            string invalidString = data[BaseDataGenerator.Rnd.Next(0, data.Count)];

            return invalidString;
        }
    }
}
