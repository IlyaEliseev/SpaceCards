using Microsoft.AspNetCore.Authentication;
using SpaceCards.API.Contracts;
using System.Security.Claims;

namespace SpaceCards.API.Services.CookieService
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CookieService> _logger;

        public CookieService(IHttpContextAccessor httpContextAccessor, ILogger<CookieService> logger)
        {
            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException($"{nameof(httpContextAccessor)} is null.");
            }

            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Extract oath user data from<see cref="HttpContext"/>.
        /// </summary>
        /// <returns>OAuth user data(<see cref = "OAuthData" />).</returns>
        public async Task<OAuthData> GetOAuthUserData()
        {
            var accessToken = await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token");
            if (accessToken is null)
            {
                _logger.LogError($"{accessToken} is null");
                return null;
            }

            var refreshToken = await _httpContextAccessor.HttpContext!.GetTokenAsync("refresh_token");
            if (refreshToken is null)
            {
                _logger.LogError($"{refreshToken} is null");
                return null;
            }

            var exp = await _httpContextAccessor.HttpContext!.GetTokenAsync("expires_at");
            if (exp is null)
            {
                _logger.LogError($"{exp} is null");
                return null;
            }

            var nickname = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (nickname is null)
            {
                _logger.LogError($"{nickname} is null");
                return null;
            }

            var email = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (email is null)
            {
                _logger.LogError($"{email} is null");
                return null;
            }

            var imageUri = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "image")?.Value;
            if (email is null)
            {
                _logger.LogError($"{imageUri} is null");
                return null;
            }

            return new OAuthData
            {
                Nickname = nickname,
                Email = email,
                ImageUri = imageUri,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpireAt = DateTime.SpecifyKind(DateTime.Parse(exp), DateTimeKind.Utc)
            };
        }

        public CookieService SetCookie(UserCookie userCookie)
        {
            if (userCookie.CookieOptions is not null)
            {
                _httpContextAccessor.HttpContext!.Response.Cookies.Append(userCookie.Key, userCookie.Value, userCookie.CookieOptions);
            }

            _httpContextAccessor.HttpContext!.Response.Cookies.Append(userCookie.Key, userCookie.Value);

            return this;
        }

        public void SetCookies(List<UserCookie> userCookies)
        {
            foreach (var userCookie in userCookies)
            {
                if (userCookie.CookieOptions is not null)
                {
                    _httpContextAccessor.HttpContext!.Response.Cookies.Append(userCookie.Key, userCookie.Value, userCookie.CookieOptions);
                }

                _httpContextAccessor.HttpContext!.Response.Cookies.Append(userCookie.Key, userCookie.Value);
            }
        }

        public CookieService DeleteCookie(string cookieName)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookieName);
            return this;
        }

        public CookieService DeleteCookie(string cookieName, CookieOptions options)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookieName, options);
            return this;
        }
    }
}
