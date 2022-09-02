namespace SpaceCards.API.Contracts
{
    // Contract for response token and refresh token.
    public class TokensResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
