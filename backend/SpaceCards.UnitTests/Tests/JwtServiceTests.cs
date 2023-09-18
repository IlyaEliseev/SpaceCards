using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using SpaceCards.API;
using SpaceCards.API.Services.JwtService;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Claims;
using Xunit;

namespace SpaceCards.UnitTests.Tests
{
    public class JwtServiceTests
    {
        private IOptions<JwtOptions> _options;

        public JwtServiceTests()
        {
            _options = Options.Create(new JwtOptions
            {
                Secret = "1234",
                Provider = new JwtNetProvider()
            });
        }

        [Fact]
        public void Decoding_jwt_token()
        {
            // arrange
            var userId = Guid.NewGuid();
            var nickname = "Tom";
            var expTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
            var claims = new Dictionary<string, object>()
            {
                { ClaimTypes.NameIdentifier, userId },
                { ClaimTypes.Name, nickname }
            };

            var sut = new JwtService(_options);

            // act
            var token = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(_options.Value.Secret)
                    .ExpirationTime(expTime)
                    .AddClaims(claims)
                    .WithVerifySignature(true)
                    .Encode();
            var decodeToken = sut.DecodeToken(token);

            // assert
            Assert.NotNull(decodeToken);
            Assert.Equal(userId, decodeToken.UserId);
            Assert.Equal(nickname, decodeToken.Nickname);
        }

        [Fact]
        public void Jwt_token_with_a_valid_parameters_is_creating()
        {
            // arrange
            var userId = Guid.NewGuid();
            var nickname = "Tom";
            var expTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
            var claims = new Dictionary<string, object>()
            {
                { ClaimTypes.NameIdentifier, userId },
                { ClaimTypes.Name, nickname }
            };

            var sut = new JwtService(_options);

            // act
            var token = sut
                .AddClaims(claims)
                .CreateToken(claims, expTime);

            var decodeToken = JwtBuilder.Create()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(_options.Value.Secret)
                        .MustVerifySignature()
                        .Decode<UserInformation>(token);

            // assert
            Assert.NotNull(token);
            Assert.Equal(userId, decodeToken.UserId);
            Assert.Equal(nickname, decodeToken.Nickname);
        }

        [Fact]
        public void Access_and_refresh_jwt_token_with_a_valid_parameters_is_creating()
        {
            // arrange
            var userId = Guid.NewGuid();
            var nickname = "Tom";
            var accessTokenExpTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
            var refreshTokenExpTime = DateTimeOffset.UtcNow.AddHours(5).ToUnixTimeSeconds();
            var claims = new Dictionary<string, object>()
            {
                { ClaimTypes.NameIdentifier, userId },
                { ClaimTypes.Name, nickname }
            };

            var sut = new JwtService(_options);

            // act
            var (accessToken, refreshToken) = sut.CreateTokens(claims, accessTokenExpTime, refreshTokenExpTime);
            var decodeAccessToken = JwtBuilder.Create()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(_options.Value.Secret)
                        .MustVerifySignature()
                        .Decode<UserInformation>(accessToken);

            var decodeRefreshToken = JwtBuilder.Create()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(_options.Value.Secret)
                        .MustVerifySignature()
                        .Decode<UserInformation>(refreshToken);

            // assert
            Assert.NotNull(accessToken);
            Assert.NotNull(refreshToken);
            Assert.Equal(userId, decodeAccessToken.UserId);
            Assert.Equal(nickname, decodeAccessToken.Nickname);
            Assert.Equal(userId, decodeRefreshToken.UserId);
            Assert.Equal(nickname, decodeRefreshToken.Nickname);
        }
    }
}
