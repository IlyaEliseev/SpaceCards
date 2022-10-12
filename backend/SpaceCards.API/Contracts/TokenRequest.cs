using SpaceCards.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    // Contract for request AccessToken and RefreshToken.
    public class TokenRequest
    {
        [MaxLength(Session.MaxLengthToken)]
        public string AccessToken { get; set; }

        [MaxLength(Session.MaxLengthToken)]
        public string RefreshToken { get; set; }
    }
}
