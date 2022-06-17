using System.Collections.Generic;

namespace SpaceCards.IntegrationTests
{
    internal class CardDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidCardId(int testCount)
        {
            for (int i = 0; i < testCount; i++)
            {
                yield return new object[]
                {
                    -BaseDataGenerator.Rnd
                    .Next(0, int.MaxValue)
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvalidCardIdFrontSideBackSide(int testCount)
        {
            var data = GetInvalidCardIdFrontSideBackSide(testCount);

            for (int i = 0; i < testCount; i++)
            {
                var (invalidCardId, invalidFrontSide, invalidBackSide) = data[i];

                yield return new object[]
                {
                    invalidCardId, invalidFrontSide, invalidBackSide
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvalidFrontSideBackSide(int testCount)
        {
            var data = GetInvalidFrontSideBackSide(testCount);

            for (int i = 0; i < testCount; i++)
            {
                var (invalidFrontSide, invalidBackSide) = data[i];

                yield return new object[]
                {
                    invalidFrontSide, invalidBackSide
                };
            }
        }

        private static List<(
            int InvalidCardId,
            string InvalidFrontSide,
            string InvalidBackSide)>
            GetInvalidCardIdFrontSideBackSide(int count)
        {
            var invalidData = new List<string>
            {
                null,
                string.Empty,
            };

            var data = new List<(int InvalidCardId, string InvalidFrontSide, string InvalidBackSide)>();

            for (int i = 0; i < count; i++)
            {
                var whiteSpaceElement = string.Empty.PadLeft(
                    BaseDataGenerator.Rnd.Next(1, 100), ' ');

                invalidData.Add(whiteSpaceElement);

                var tuple = (
                    -BaseDataGenerator.Rnd.Next(0, int.MaxValue),
                    invalidData[BaseDataGenerator.Rnd.Next(0, invalidData.Count)],
                    invalidData[BaseDataGenerator.Rnd.Next(0, invalidData.Count)]);

                data.Add(tuple);
            }

            return data;
        }

        private static List<(string InvalidFrontSide, string InvalidBackSide)> GetInvalidFrontSideBackSide(int count)
        {
            var invalidData = new List<string>
            {
                null,
                string.Empty,
            };

            var data = new List<(string InvalidFrontSide, string InvalidBackSide)>();

            for (int i = 0; i < count; i++)
            {
                var whiteSpaceElement = string.Empty.PadLeft(
                    BaseDataGenerator.Rnd.Next(1, 100), ' ');

                invalidData.Add(whiteSpaceElement);

                var tuple = (
                    invalidData[BaseDataGenerator.Rnd.Next(0, invalidData.Count)],
                    invalidData[BaseDataGenerator.Rnd.Next(0, invalidData.Count)]);

                data.Add(tuple);
            }

            return data;
        }
    }
}
