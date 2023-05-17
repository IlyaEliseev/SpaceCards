namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class OAuthUserTokenEntity
    {
        public int Id { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpireTime { get; set; }

        public Guid OAuthUserId { get; set; }

        public OAuthUserEntity? User { get; set; }
    }
}
