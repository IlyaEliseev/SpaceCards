namespace SpaceCards.API.Contracts
{
    // Contract for response token and refresh token.
    public class TokensResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string Nickname { get; set; }
    }
}
