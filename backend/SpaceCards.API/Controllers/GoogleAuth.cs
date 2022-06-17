using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace SpaceCards.API.Controllers
{
    public class GoogleAuth : BaseApiController
    {
        [HttpGet("/signin-google")]
        public async Task<IActionResult> SignIn([FromQuery] Dictionary<string, string> request)
        {
            return Ok(request);
        }
    }
    //https://localhost:49394/signin-google?
          //state=CfDJ8OWcWp5SE1BLga9mQEckHXCuuTugOZ4rASVxvOTcWTjb7nirvAQ1bTZfa5QY5zSzBrlQDbWYPqXdn3wDsSQsV7CkaZhDwK3shMoVKySMsFkprgxT_cLXUnt_aj6N5BpSb7tsjYidaj5Rd7IkTPS3WGsFwp-I8XbBxYbVAoQ2eNqHwaUdO2p5TnSuWu0inDqaGq0Ki6ai7PO4ksBy68UBPmA&code=4%2F0AX4XfWiq4m10CocymBPgxY0l61YB2QQ2A1AP5ZOJ7w57csD_bKWT0tpUhG4dM9BZwYdGpA
          //&scope=email+profile+openid+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.email
          //&authuser=0
          //&prompt=consent
}
