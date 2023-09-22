using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SpaceCards.UnitTests.MemberData
{
    public class CookieServiceTestsDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidAuthData()
        {
            var name = "Name";
            var email = "test@gmail.com";
            var imageUri = "image.jpg";
            var accessToken = "Access_Token";
            var refreshToken = "Refresh_Token";
            var expireAt = DateTime.UtcNow.ToString();

            var allClaims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name),
                new Claim("image", imageUri)
            };

            var claimsWithoutEmail = new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim("image", imageUri)
            };

            var claimsWithoutNickname = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("image", imageUri)
            };

            var claimsWithoutImageUri = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name),
            };

            yield return new object[] { claimsWithoutEmail, accessToken, refreshToken, expireAt };
            yield return new object[] { claimsWithoutNickname, accessToken, refreshToken, expireAt };
            yield return new object[] { claimsWithoutImageUri, accessToken, refreshToken, expireAt };
            yield return new object[] { allClaims, null, refreshToken, expireAt };
            yield return new object[] { allClaims, accessToken, null, expireAt };
            yield return new object[] { allClaims, accessToken, refreshToken, null };
        }
    }
}
