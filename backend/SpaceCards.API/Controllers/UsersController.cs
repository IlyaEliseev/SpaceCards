using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace SpaceCards.API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly JWTSecretOptions _options;

        public UsersController(IOptions<JWTSecretOptions> options)
        {
            _options = options.Value;
        }

        [AllowAnonymous]
        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {
            var userId = Guid.NewGuid();

            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm())
                      .WithSecret(_options.Secret)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(3).ToUnixTimeSeconds())
                      .AddClaim(ClaimTypes.NameIdentifier, userId)
                      .Encode();

            return Ok(token);
        }

        [Authorize(AuthenticationSchemes = BaseSchema.NAME)]
        [HttpGet("validate")]
        public async Task<IActionResult> Validate(string token)
        {
            var userId = UserId;

            var json = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm())
                     .WithSecret(_options.Secret)
                     .MustVerifySignature()
                     .Decode(token);

            return Ok(json);
        }
    }
}
