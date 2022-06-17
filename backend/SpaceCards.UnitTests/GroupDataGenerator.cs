using SpaceCards.Domain;
using System.Collections.Generic;

namespace SpaceCards.UnitTests
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
            var data = GetInvalidName(testCount);

            for (int i = 0; i < testCount; i++)
            {
                yield return new object[]
                {
                    data[i]
                };
            }
        }

        public static IEnumerable<object[]> GenerateSetNameWithInvalidLength(int testCount)
        {
            for (int i = 0; i < testCount; i++)
            {
                var length = BaseDataGenerator.Rnd
                    .Next(
                    Group.MAX_NAME_LENGTH + 1,
                    int.MaxValue / 1000);

                yield return new object[]
                {
                    StringFixture.GenerateRandomString(length)
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
                var whiteSpace = string.Empty.PadLeft(
                    BaseDataGenerator.Rnd.Next(1, 100), ' ');

                if (!data.Contains(whiteSpace))
                {
                    data.Add(whiteSpace);
                }
            }

            return data;
        }
    }
}
