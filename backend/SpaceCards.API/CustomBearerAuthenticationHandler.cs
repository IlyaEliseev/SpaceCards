using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SpaceCards.API;
using SpaceCards.API.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

public class CustomBearerAuthenticationHandler : AuthenticationHandler<CustomBearerAuthenticationOption>
{
    private readonly JWTSecretOptions _secretOption;

    public CustomBearerAuthenticationHandler(
        IOptionsMonitor<CustomBearerAuthenticationOption> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IOptions<JWTSecretOptions> secretOption)
        : base(options, logger, encoder, clock)
    {
        _secretOption = secretOption.Value;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
        {
            return AuthenticateResult.Fail("Header Not Found.");
        }

        if (!AuthenticationHeaderValue.TryParse(Request.Headers[HeaderNames.Authorization], out var headerValue))
        {
            return AuthenticateResult.NoResult();
        }

        if (!JwtBearerDefaults.AuthenticationScheme.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.NoResult();
        }

        var tokenClaims = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm())
                     .WithSecret(_secretOption.Secret)
                     .MustVerifySignature()
                     .Decode<IDictionary<string, string>>(headerValue.Parameter);

        tokenClaims.TryGetValue(ClaimTypes.NameIdentifier, out var userId);

        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };

        var claimsIdentity = new ClaimsIdentity(claims, BaseSchema.NAME);

        var principal = new ClaimsPrincipal(claimsIdentity);

        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
