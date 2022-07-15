using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace SpaceCards.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class BaseApiController : ControllerBase
    {
        protected Result<Guid> UserId
        {
            get
            {
                var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (claim is null)
                {
                    return Result.Failure<Guid>($"{nameof(claim)} cannot be null");
                }

                if (!Guid.TryParse(claim.Value, out var userId))
                {
                    return Result.Failure<Guid>($"Cannot parse userId: {claim.Value}.");
                }

                return userId;
            }
        }
    }
}
