using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.UnitTests.MemberData
{
    public class BaseDataGenerator
    {
        public static string MakeInvalidString(params string[] invalidData)
        {
            var rnd = new Random();

            var data = new List<string>
            {
                null,
                string.Empty
            };

            data.AddRange(invalidData ?? Array.Empty<string>());

            var whiteSpaces = Enumerable.Range(0, 5)
                .Select(x => string.Empty.PadLeft(
                rnd.Next(1, 100), ' ')).ToArray();

            data.AddRange(whiteSpaces);

            string invalidString = data[rnd.Next(0, data.Count)];

            return invalidString;
        }

        public static int MakeInvalidSuccess()
        {
            var rnd = new Random();

            var successValid = new List<int>
            {
                0, 1
            };

            var invalidSuccess = Enumerable.Range(0, 10)
                .Select(x => rnd.Next(int.MinValue, int.MaxValue))
                .Where(x => x != successValid[0] && x != successValid[1])
                .ToArray();

            return invalidSuccess[rnd.Next(0, invalidSuccess.Count())];
        }
    }
}
