using System.Collections.Generic;

namespace SpaceCards.IntegrationTests
{
    internal class GuessedCardsDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidCardIdGroupId(int testCount)
        {
            var data = GetInvalidCardIdGroupId(testCount);

            for (int i = 0; i < testCount; i++)
            {
                var (invalidCardId, invalidGroupId) = data[i];

                yield return new object[]
                {
                    invalidCardId, invalidGroupId
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvalidCardIdOrGroupId(int testCount)
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

        private static List<(int IvalidCardId, int InvalidGroupId)> GetInvalidCardIdGroupId(int count)
        {
            var data = new List<(int IvalidCardId, int InvalidGroupId)>();

            for (int i = 0; i < count; i++)
            {
                var tuple = (
                    -BaseDataGenerator.Rnd.Next(0, int.MaxValue),
                    -BaseDataGenerator.Rnd.Next(0, int.MaxValue));

                data.Add(tuple);
            }

            return data;
        }
    }
}
