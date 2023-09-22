using Microsoft.AspNetCore.Mvc;
using SpaceCards.API.Contracts;
using Microsoft.Extensions.Options;
using SpaceCards.BusinessLogic;
using SpaceCards.Domain.Interfaces;
using SpaceCards.API.Options;
using SpaceCards.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using SpaceCards.API.Services.JwtService;
using System.Security.Claims;
using SpaceCards.API.Services.CookieService;
using Octokit;

namespace SpaceCards.API.Controllers
{
    [AllowAnonymous]
    public class UsersAccountController : BaseApiController
    {
        private readonly ILogger<UsersAccountController> _logger;
        private readonly IUsersAccountService _userAccountService;
        private readonly SecurityService _securityService;
        private readonly JWTSecretOptions _options;
        private readonly ISessionsRepository _sessionsRepository;
        private readonly JwtService _jwtService;
        private readonly ICookieService _cookieService;

        public UsersAccountController(
            ILogger<UsersAccountController> logger,
            IUsersAccountService userAccountService,
            IOptions<JWTSecretOptions> options,
            SecurityService securityService,
            ISessionsRepository sessionsRepository,
            JwtService jwtService,
            ICookieService cookieService)
        {
            _logger = logger;
            _userAccountService = userAccountService;
            _securityService = securityService;
            _options = options.Value;
            _sessionsRepository = sessionsRepository;
            _jwtService = jwtService;
            _cookieService = cookieService;
        }

        /// <summary>
        /// Registration users.
        /// </summary>
        /// <param name="request">Users email and password.</param>
        /// <returns>Success.</returns>
        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationRequest request)
        {
            var user = await _userAccountService.GetByEmail(request.Email);
            if (user.IsFailure)
            {
                var userEntity = new DataAccess.Postgre.Entites.UserEntity
                {
                    Nickname = request.Nickname,
                    Email = request.Email,
                    PasswordHash = request.Password
                };

                var passwordHash = _securityService.HashPassword(userEntity, request.Password);

                var newUser = Domain.Model.User.Create(
                    userEntity.Email,
                    passwordHash,
                    request.Nickname);

                if (newUser.IsFailure)
                {
                    _logger.LogError("{errors}", newUser.Error);
                    return BadRequest(newUser.Error);
                }

                var result = await _userAccountService.Create(newUser.Value);
                if (result.IsFailure)
                {
                    _logger.LogError("{errors}", result.Error);
                    return BadRequest(result.Error);
                }

                return Ok(result.Value);
            }
            else
            {
                var exception = "user already exists.";
                _logger.LogError("{errors}", exception);
                return BadRequest(exception);
            }
        }

        /// <summary>
        /// Login users.
        /// </summary>
        /// <param name="request">Email and password.</param>
        /// <returns>Token and refresh token.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokensResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userAccountService.GetByEmail(request.Email);
            if (user.IsFailure)
            {
                _logger.LogError("{errors}", user.Error);
                return BadRequest(user.Error);
            }

            var userEntity = new DataAccess.Postgre.Entites.UserEntity
            {
                Email = user.Value.Email,
                PasswordHash = user.Value.PasswordHash
            };

            var resultVerify = _securityService.VerifyPassword(userEntity, request.Password);
            if (!resultVerify)
            {
                _logger.LogError("{errors}", "incorrect password");
                return BadRequest("incorrect password");
            }

            var (accessToken, refreshToken) = _jwtService.CreateTokens(
                claims: new Dictionary<string, object>()
                {
                    { ClaimTypes.NameIdentifier, user.Value.UserId },
                    { ClaimTypes.Name, user.Value.Nickname }
                },
                accessTokenExpireTime: DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds(),
                refreshTokenExpireTime: DateTimeOffset.UtcNow.AddDays(30).ToUnixTimeSeconds());

            var session = Session.Create(user.Value.UserId, accessToken, refreshToken);
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

            var accessTokenCookieOptions = new CookieOptions() { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None };
            var otherCookieOptions = new CookieOptions() { Secure = true, SameSite = SameSiteMode.None };

            _cookieService
                .SetCookie(new UserCookie() { Key = "_sp_i", Value = accessToken, CookieOptions = accessTokenCookieOptions })
                .SetCookie(new UserCookie() { Key = "nickname", Value = user.Value.Nickname, CookieOptions = otherCookieOptions })
                .SetCookie(new UserCookie() { Key = "session_id", Value = $"{Guid.NewGuid()}", CookieOptions = otherCookieOptions });

            return Ok();
        }

        /// <summary>
        /// Refresh access token.
        /// </summary>
        /// <param name="request">Access token and refresh token.</param>
        /// <returns>New access token and refresh token.</returns>
        [HttpPost("refreshaccesstoken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokensResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshAccessToken([FromBody] TokenRequest request)
        {
            var userInformation = _jwtService.DecodeToken(request.RefreshToken);

            var resultGet = await _sessionsRepository.GetById(userInformation.UserId);
            if (resultGet.IsFailure)
            {
                _logger.LogError("{error}", resultGet.Error);
                return BadRequest(resultGet.Error);
            }

            var (accsessToken, refreshToken) = _jwtService.CreateTokens(
                claims: new Dictionary<string, object>()
                {
                    { ClaimTypes.NameIdentifier, userInformation.UserId },
                    { ClaimTypes.Name, userInformation.Nickname }
                },
                accessTokenExpireTime: DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds(),
                refreshTokenExpireTime: DateTimeOffset.UtcNow.AddDays(30).ToUnixTimeSeconds());

            var session = Session.Create(userInformation.UserId, accsessToken, refreshToken);
            if (resultGet.IsFailure)
            {
                _logger.LogError("{error}", resultGet.Error);
                return BadRequest(resultGet.Error);
            }

            var result = await _sessionsRepository.Create(resultGet.Value);
            if (result.IsFailure)
            {
                _logger.LogError("{error}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(new TokensResponse
            {
                AccessToken = accsessToken,
                RefreshToken = refreshToken,
                Nickname = userInformation.Nickname
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var accessTokenCookieOptions = new CookieOptions() { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None };
            var otherCookieOptions = new CookieOptions() { Secure = true, SameSite = SameSiteMode.None };

            _cookieService
                .DeleteCookie("_sp_i", accessTokenCookieOptions)
                .DeleteCookie("nickname", otherCookieOptions)
                .DeleteCookie("avatar", otherCookieOptions)
                .DeleteCookie("session_id", otherCookieOptions);

            return Ok();
        }
    }
}
