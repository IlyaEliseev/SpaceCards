using SpaceCards.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.IntegrationTests.MemberData
{
    internal class BaseDataGenerator
    {
        public static int GetRandomLengthString()
        {
            var rnd = new Random();

            var length = rnd.Next(
                    Group.MAX_NAME_LENGTH + 1,
                    int.MaxValue / 1000);

            return length;
        }

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
    }
}
