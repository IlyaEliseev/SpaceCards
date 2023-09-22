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
using SpaceCards.API.Services.JwtService;
using SpaceCards.API.Services.CookieService;

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
        private readonly ICookieService _cookieService;

        public OAuthController(
            ILogger<OAuthController> logger,
            IOptions<JWTSecretOptions> jwtSecretOptions,
            IOAuthUsersService oAuthUsersService,
            IOAuthUserTokensService oAuthUserTokensService,
            ISessionsRepository sessionsRepository,
            JwtService jwtService,
            ICookieService cookieService)
        {
            _jwtSecretOptions = jwtSecretOptions.Value;
            _logger = logger;
            _oAuthUsersService = oAuthUsersService;
            _oAuthUserTokensService = oAuthUserTokensService;
            _sessionsRepository = sessionsRepository;
            _jwtService = jwtService;
            _cookieService = cookieService;
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
            var oauthParameters = await _cookieService.GetOAuthUserData();
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

            var accessTokenCookieOptions = new CookieOptions() { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None };
            var otherCookieOptions = new CookieOptions() { Secure = true, SameSite = SameSiteMode.None };

            _cookieService
                .SetCookie(new UserCookie() { Key = "_sp_i", Value = accessToken, CookieOptions = accessTokenCookieOptions })
                .SetCookie(new UserCookie() { Key = "nickname", Value = oauthParameters.Nickname, CookieOptions = otherCookieOptions })
                .SetCookie(new UserCookie() { Key = "avatar", Value = oauthParameters.ImageUri, CookieOptions = otherCookieOptions })
                .SetCookie(new UserCookie() { Key = "session_id", Value = $"{Guid.NewGuid()}", CookieOptions = otherCookieOptions });

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

            var accessTokenCookieOptions = new CookieOptions() { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None };
            var otherCookieOptions = new CookieOptions() { Secure = true, SameSite = SameSiteMode.None };

            _cookieService
                .DeleteCookie("_sp_i", accessTokenCookieOptions)
                .DeleteCookie("nickname", otherCookieOptions)
                .DeleteCookie("avatar", otherCookieOptions)
                .DeleteCookie("session_id", otherCookieOptions);

            return Redirect("http://localhost:3000");
        }
    }
}
