﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCards.UnitTests
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
    }
}