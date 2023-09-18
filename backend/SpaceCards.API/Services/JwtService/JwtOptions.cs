using Microsoft.Extensions.Options;

namespace SpaceCards.API.Services.JwtService
{

    public class JwtOptions
    {
        public string Secret { get; set; }

        public IJwtTokenProvider Provider { get; set; }
    }
}
