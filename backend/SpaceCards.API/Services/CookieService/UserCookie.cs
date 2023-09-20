using Microsoft.AspNetCore.Authentication;
using SpaceCards.API.Contracts;
using System.Security.Claims;

namespace SpaceCards.API.Services.CookieService
{

    public class UserCookie
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public CookieOptions? CookieOptions { get; set; }
    }
}
