using CSharpFunctionalExtensions;
using JWT.Algorithms;
using JWT.Builder;
using SpaceCards.API.Options;
using System.Security.Claims;

namespace SpaceCards.API
{
    public class JwtHelper
    {
        public static string CreateAccessToken(
            UserInformation information,
            JWTSecretOptions options)
        {
            var accsessToken = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm())
                      .WithSecret(options.Secret)
                      .ExpirationTime(DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim(ClaimTypes.Name, information.Nickname)
                      .AddClaim(ClaimTypes.NameIdentifier, information.UserId)
                      .WithVerifySignature(true)
                      .Encode();

            return accsessToken;
        }

        public static string CreateRefreshToken(
            UserInformation information,
            JWTSecretOptions options)
        {
            var refreshToken = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(options.Secret)
                    .ExpirationTime(DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeSeconds())
                    .AddClaim(ClaimTypes.Name, information.Nickname)
                    .AddClaim(ClaimTypes.NameIdentifier, information.UserId)
                    .WithVerifySignature(true)
                    .Encode();

            return refreshToken;
        }

        public static IDictionary<string, object> GetPayloadFromJWTToken(
            string token,
            JWTSecretOptions options)
        {
            var payload = JwtBuilder.Create()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(options.Secret)
                        .MustVerifySignature()
                        .Decode<IDictionary<string, object>>(token);

            return payload;
        }

        public static Result<UserInformation> ParseUserInformation(
            IDictionary<string,
                object> payload)
        {
            Result failure = Result.Success();

            if (!payload.TryGetValue(
                ClaimTypes.NameIdentifier,
                out var nameIdentifierValue))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<UserInformation>("User id is not found."));
            }

            if (!Guid.TryParse((string)nameIdentifierValue, out var userId))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<UserInformation>(
                        $"{nameof(userId)} is can't parsing."));
            }

            if (!payload.TryGetValue(ClaimTypes.Name, out var nicknameValue))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<UserInformation>("Nickname is not found."));
            }

            var nickname = (string)nicknameValue;
            if (string.IsNullOrWhiteSpace(nickname))
            {
                failure = Result.Combine(
                    failure,
                    Result.Failure<UserInformation>(
                        $"{nameof(nickname)} is can't parsing."));
            }

            if (failure.IsFailure)
            {
                return Result.Failure<UserInformation>(failure.Error);
            }

            return new UserInformation(nickname, userId);
        }
    }
}
