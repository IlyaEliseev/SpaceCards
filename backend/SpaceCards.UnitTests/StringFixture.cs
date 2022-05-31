using System;
using System.Linq;

namespace SpaceCards.UnitTests
{
    public sealed class StringFixture
    {
        private readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public string GenerateRandomString(int length)
        {
            var rnd = new Random();
            var result = new string(Enumerable.Repeat(_chars, length)
                .Select(x => x[rnd.Next(x.Length)])
                .ToArray());

            return result;
        }
    }
}
