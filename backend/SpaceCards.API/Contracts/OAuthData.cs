namespace SpaceCards.API.Contracts
{
    public class OAuthData
    {
        public string Nickname { get; set; }

        public string Email { get; set; }

        public string ImageUri { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
