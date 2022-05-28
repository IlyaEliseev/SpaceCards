using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace SpaceCards.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class BaseApiController : ControllerBase
    {
    }
}
