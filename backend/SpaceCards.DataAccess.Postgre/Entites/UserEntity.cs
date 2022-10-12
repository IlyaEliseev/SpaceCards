namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class UserEntity
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string Nickname { get; set; }

        public string PasswordHash { get; set; }

        public DateTime RegistrationData { get; set; }
    }
}
