namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class SessionEntity
    {
        public Guid UserId { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
