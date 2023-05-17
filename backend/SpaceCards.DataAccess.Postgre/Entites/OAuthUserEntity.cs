namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class OAuthUserEntity
    {
        public Guid Id { get; set; }

        public string Nickname { get; set; }

        public string Email { get; set; }

        public DateTime RegistrationData { get; set; }

        public DateTime? DeleteDate { get; set; }

        public OAuthUserTokenEntity? Token { get; set; }
    }
}
