using Microsoft.Extensions.Options;

namespace SpaceCards.API.Services.JwtService
{
    public class JwtService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtService(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }

        public string CreateToken(IDictionary<string, object> claims, long expireTime)
        {
            return _jwtOptions.Provider.CreateToken(claims, expireTime, _jwtOptions.Secret);
        }

        public (string AccessToken, string RefreshToken) CreateTokens(
            IDictionary<string, object> claims,
            long accessTokenExpireTime,
            long refreshTokenExpireTime)
        {
            var accessToken = _jwtOptions.Provider.CreateToken(claims, accessTokenExpireTime, _jwtOptions.Secret);
            var refreshToken = _jwtOptions.Provider.CreateToken(claims, refreshTokenExpireTime, _jwtOptions.Secret);

            return (accessToken, refreshToken);
        }

        public UserInformation DecodeToken(string token)
        {
            return _jwtOptions.Provider.DecodeClaimsFromToken(token, _jwtOptions.Secret);
        }
    }
}
