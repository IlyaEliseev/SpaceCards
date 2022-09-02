using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using SpaceCards.API.Contracts;
using SpaceCards.Domain;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using SpaceCards.BusinessLogic;

namespace SpaceCards.API.Controllers
{
    public class UsersAccountController : BaseApiController
    {
        private readonly ILogger<UsersAccountController> _logger;
        private readonly IUsersAccountService _userAccountService;
        private readonly SecurityService _securityService;
        private readonly JWTSecretOptions _options;

        public UsersAccountController(
            ILogger<UsersAccountController> logger,
            IUsersAccountService userAccountService,
            IOptions<JWTSecretOptions> options,
            SecurityService securityService)
        {
            _logger = logger;
            _userAccountService = userAccountService;
            _securityService = securityService;
            _options = options.Value;
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
            var user = await _userAccountService.FindUserByEmail(request.Email);
            if (user.IsFailure)
            {
                var userEntity = new DataAccess.Postgre.Entites.User
                {
                    Email = request.Email,
                    PasswordHash = request.Password
                };

                var passwordHash = _securityService.HashPassword(userEntity, request.Password);
                var newUser = Domain.User.Create(userEntity.Email, passwordHash, DateTime.UtcNow);
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
            var user = await _userAccountService.FindUserByEmail(request.Email);
            if (user.IsFailure)
            {
                _logger.LogError("{errors}", user.Error);
                return BadRequest(user.Error);
            }

            var userEntity = new DataAccess.Postgre.Entites.User
            {
                Email = user.Value.Email,
                PasswordHash = user.Value.PasswordHash
            };

            var result = _securityService.VerifyPassword(userEntity, request.Password);
            if (!result)
            {
                _logger.LogError("{errors}", "incorrect password");
                return BadRequest("incorrect password");
            }

            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm())
                      .WithSecret(_options.Secret)
                      .ExpirationTime(DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds())
                      .AddClaim(ClaimTypes.NameIdentifier, user.Value.UserId)
                      .WithVerifySignature(true)
                      .Encode();

            var refreshToken = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm())
                      .WithSecret(_options.Secret)
                      .ExpirationTime(DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds())
                      .AddClaim(ClaimTypes.NameIdentifier, user.Value.UserId)
                      .WithVerifySignature(true)
                      .Encode();

            var tokens = new TokensResponse
            {
                Token = token,
                RefreshToken = refreshToken
            };

            return Ok(tokens);
        }
    }
}
