using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SpaceCards.API.Controllers
{
    public class UsersController : BaseApiController
    {
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = BaseSchema.NAME)]
        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {
            var userId = 12;

            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm())
                      .WithSecret("secret")
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(3).ToUnixTimeSeconds())
                      .AddClaim("claim2", "claim2-value")
                      .AddClaim(ClaimTypes.NameIdentifier, userId)
                      .Encode();

            var user = User;
            var isAuth = User.Identity.IsAuthenticated;
            return Ok(token);
        }

        [Authorize(AuthenticationSchemes = BaseSchema.NAME)]
        [HttpGet("validate")]
        public async Task<IActionResult> Validate(string token)
        {
            var json = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm())
                     .WithSecret("secret")
                     .MustVerifySignature()
                     .Decode(token);

            return Ok(json);
        }
    }
}
