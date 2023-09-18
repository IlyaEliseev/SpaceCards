using AspNet.Security.OAuth.MailRu;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using SpaceCards.API.Options;
using SpaceCards.Domain.Model;
using SpaceCards.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SpaceCards.API.Contracts;
using SpaceCards.API.Services.JwtService;

namespace SpaceCards.API.Controllers
{
    [AllowAnonymous]
    public class OAuthController : BaseApiController
    {
        private readonly ILogger<OAuthController> _logger;
        private readonly JWTSecretOptions _jwtSecretOptions;
        private readonly IOAuthUsersService _oAuthUsersService;
        private readonly IOAuthUserTokensService _oAuthUserTokensService;
        private readonly ISessionsRepository _sessionsRepository;
        private readonly JwtService _jwtService;

        public OAuthController(
            ILogger<OAuthController> logger,
            IOptions<JWTSecretOptions> jwtSecretOptions,
            IOAuthUsersService oAuthUsersService,
            IOAuthUserTokensService oAuthUserTokensService,
            ISessionsRepository sessionsRepository,
            JwtService jwtService)
        {
            _jwtSecretOptions = jwtSecretOptions.Value;
            _logger = logger;
            _oAuthUsersService = oAuthUsersService;
            _oAuthUserTokensService = oAuthUserTokensService;
            _sessionsRepository = sessionsRepository;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Challenge google auth scheme <see cref="GoogleDefaults.AuthenticationScheme"/> and redirect to login endpoint <see cref="Login"/>.
        /// </summary>
        [HttpGet("google")]
        public async Task Google()
        {
            await HttpContext.ChallengeAsync(
                GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = "https://localhost:49394/oauth/login" });
        }

        /// <summary>
        /// Challenge mailru auth scheme <see cref="MailRuAuthenticationDefaults.AuthenticationScheme"/> and redirect to login endpoint <see cref="Login"/>.
        /// </summary>
        [HttpGet("mailru")]
        public async Task MailRu()
        {
            await HttpContext.ChallengeAsync(
                MailRuAuthenticationDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = "https://localhost:49394/oauth/login" });
        }

        /// <summary>
        /// Generate jwt token <see cref="JwtHelper"/> with oauth parameters and store in cookie.
        /// </summary>
        /// <returns>Redirect (<see cref="StatusCodes.Status302Found"/>) to specified url.</returns>
        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            var oauthParameters = await GetOAuthUserData();
            if (oauthParameters is null)
            {
                _logger.LogError("{error}", "One of oauth parameters is null");
                return BadRequest("Login is failed");
            }

            var newUser = OAuthUser.Create(oauthParameters.Nickname, oauthParameters.Email);
            var oauthUserId = await _oAuthUsersService.CreateOAuthUser(newUser.Value);
            if (oauthUserId.IsFailure)
            {
                _logger.LogError("{error}", oauthUserId.Error);
                return BadRequest(oauthUserId.Error);
            }

            var (accessToken, refreshToken) = _jwtService.CreateTokens(
                claims: new Dictionary<string, object>()
                {
                    { ClaimTypes.NameIdentifier, oauthUserId.Value },
                    { ClaimTypes.Name, oauthParameters.Nickname }
                },
                accessTokenExpireTime: DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds(),
                refreshTokenExpireTime: DateTimeOffset.UtcNow.AddDays(30).ToUnixTimeSeconds());

            var session = Session.Create(oauthUserId.Value, accessToken, refreshToken);
            if (session.IsFailure)
            {
                _logger.LogError("{error}", session.Error);
                return BadRequest(session.Error);
            }

            var result = await _sessionsRepository.Create(session.Value);
            if (result.IsFailure)
            {
                _logger.LogError("{error}", result.Error);
                return BadRequest(result.Error);
            }

            var oauthUserToken = OAuthUserToken.Create(
                oauthUserId.Value,
                oauthParameters.AccessToken,
                oauthParameters.RefreshToken,
                oauthParameters.ExpireAt);

            var oauthUserTokenId = await _oAuthUserTokensService.CreateOAuthUserToken(oauthUserToken.Value);
            if (oauthUserTokenId.IsFailure)
            {
                _logger.LogError("{error}", oauthUserTokenId.Error);
                return BadRequest(oauthUserTokenId.Error);
            }

            SetCookie(accessToken, oauthParameters.Nickname, oauthParameters.ImageUri);

            return Redirect("http://localhost:3000");
        }

        /// <summary>
        /// Delete cookie with jwt token.
        /// </summary>
        /// <returns><returns>Redirect (<see cref="StatusCodes.Status302Found"/>) to specified url.</returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("nickname");
            HttpContext.Response.Cookies.Delete("avatar");
            HttpContext.Response.Cookies.Delete("session_id");

            return Redirect("http://localhost:3000");
        }

        /// <summary>
        /// Extract oath user data from<see cref="HttpContext"/>.
        /// </summary>
        /// <returns>OAuth user data(<see cref = "OAuthData" />).</returns>
        [NonAction]
        public async Task<OAuthData> GetOAuthUserData()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken is null)
            {
                _logger.LogError($"{accessToken} is null");
                return null;
            }

            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            if (refreshToken is null)
            {
                _logger.LogError($"{refreshToken} is null");
                return null;
            }

            var exp = await HttpContext.GetTokenAsync("expires_at");
            if (exp is null)
            {
                _logger.LogError($"{exp} is null");
                return null;
            }

            var nickname = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (nickname is null)
            {
                _logger.LogError($"{nickname} is null");
                return null;
            }

            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (email is null)
            {
                _logger.LogError($"{email} is null");
                return null;
            }

            var imageUri = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "image")?.Value;
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

        /// <summary>
        /// Set cookie.
        /// </summary>
        /// <param name="accsessToken">Jwt access token user.</param>
        /// <param name="nickname">Nickname user.</param>
        /// <param name="imageUri">Image uri user.</param>
        [NonAction]
        public void SetCookie(string accsessToken, string nickname, string imageUri)
        {
            HttpContext.Response.Cookies.Append(
            "_sp_i",
            accsessToken,
            new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            var cookieOptions = new CookieOptions()
            {
                Secure = true,
                SameSite = SameSiteMode.None,
            };

            HttpContext.Response.Cookies.Append("nickname", nickname, cookieOptions);
            HttpContext.Response.Cookies.Append("avatar", imageUri, cookieOptions);
            HttpContext.Response.Cookies.Append("session_id", $"{Guid.NewGuid()}", cookieOptions);
        }
    }
}
