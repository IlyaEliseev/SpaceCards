namespace SpaceCards.API.Services.JwtService
{
    public interface IJwtTokenProvider
    {
        /// <summary>
        /// Create jwt token.
        /// </summary>
        /// <param name="claims">User claims.</param>
        /// <param name="expTime">Token expire time.</param>
        /// <param name="secret">Jwt token secret.</param>
        /// <returns>Jwt token as <see cref="string"/>.</returns>
        string CreateToken(IDictionary<string, object> claims, long expTime, string secret);

        /// <summary>
        /// Decode claims from jwt token.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <param name="secret">Jwt token secret.</param>
        /// <returns>Return instance <see cref="UserInformation"/>.</returns>
        UserInformation DecodeClaimsFromToken(string token, string secret);
    }
}
