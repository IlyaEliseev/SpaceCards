using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SpaceCards.UnitTests
{
    public static class IHeaderDictionaryExtensions
    {
        public static string GetCookieTestValue(this IHeaderDictionary header)
        {
            return header
                .SingleOrDefault(x => x.Key == "Set-Cookie")
                .Value
                .First()
                .Split(";")
                .First()
                .Split("=")[1];
        }

        public static string GetCookieTestOptions(this IHeaderDictionary header)
        {
            return header
                .SingleOrDefault(x => x.Key == "Set-Cookie")
                .Value
                .First()
                .Split(";")[2]
                .Trim();
        }
    }
}
