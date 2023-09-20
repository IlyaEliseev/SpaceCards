using SpaceCards.API.Contracts;

namespace SpaceCards.API.Services.CookieService
{
    public interface ICookieService
    {
        Task<OAuthData> GetOAuthUserData();

        CookieService SetCookie(UserCookie userCookie);

        void SetCookies(List<UserCookie> userCookies);

        CookieService DeleteCookie(string cookieName);

        CookieService DeleteCookie(string cookieName, CookieOptions options);
    }
}