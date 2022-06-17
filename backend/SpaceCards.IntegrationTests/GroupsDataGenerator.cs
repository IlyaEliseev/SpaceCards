using System.Collections.Generic;

namespace SpaceCards.IntegrationTests
{
    internal class GroupsDataGenerator
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

        public static IEnumerable<object[]> GenerateSetInvalidGroupIdName(int testCount)
        {
            var data = GetInvalidGroupIdName(testCount);

            for (int i = 0; i < testCount; i++)
            {
                var (invalidGroupoId, invalidName) = data[i];

                yield return new object[]
                {
                    invalidGroupoId, invalidName
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvalidName(int testCount)
        {
            var data = GetInvalidName(testCount);

            for (int i = 0; i < testCount; i++)
            {
                yield return new object[]
                {
                    data[i]
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetInvaliCountCards(int testCount)
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

        private static List<string> GetInvalidName(int count)
        {
            var data = new List<string>
            {
                null,
                string.Empty
            };

            for (int i = 0; i < count; i++)
            {
                var element = string.Empty.PadLeft(
                    BaseDataGenerator.Rnd.Next(1, 100), ' ');

                if (!data.Contains(element))
                {
                    data.Add(element);
                }
            }

            return data;
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

        private static List<(int InvalidGroupId, string invalidName)> GetInvalidGroupIdName(int count)
        {
            var data = new List<(int InvalidGroupId, string invalidName)>();

            for (int i = 0; i < count; i++)
            {
                var tuple = (
                    -BaseDataGenerator.Rnd.Next(0, int.MaxValue),
                    string.Empty.PadLeft(BaseDataGenerator.Rnd.Next(1, 100), ' '));

                data.Add(tuple);
            }

            return data;
        }

        public static IEnumerable<object[]> GenerateSetInvalidGroupId(int testCount)
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
    }
}
