using SpaceCards.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    // Contract for user registration.
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(User.MaAX_NICKNAME_LENGTH)]
        public string Nickname { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
    }
}
