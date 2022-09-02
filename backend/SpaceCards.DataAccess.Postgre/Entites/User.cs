namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime RegistrationData { get; set; }
    }
}
