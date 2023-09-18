using JWT.Algorithms;
using JWT.Builder;

namespace SpaceCards.API.Services.JwtService
{
    public class JwtNetProvider : IJwtTokenProvider
    {
        public UserInformation DecodeClaimsFromToken(string token, string secret)
        {
            return JwtBuilder.Create()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(secret)
                        .MustVerifySignature()
                        .Decode<UserInformation>(token);
        }

        public string CreateToken(IDictionary<string, object> claims, long expTime, string secret)
        {
            return JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret)
                    .ExpirationTime(expTime)
                    .AddClaims(claims)
                    .WithVerifySignature(true)
                    .Encode();
        }
    }
}
