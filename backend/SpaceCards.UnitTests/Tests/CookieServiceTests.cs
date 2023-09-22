using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Moq;
using SpaceCards.API.Services.CookieService;
using SpaceCards.UnitTests.MemberData;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests.Tests
{
    public class CookieServiceTests
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<CookieService> _logger;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;

        public CookieServiceTests()
        {
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(defaultContext);
            _contextAccessor = _mockHttpContextAccessor.Object;
            _logger = new NullLogger<CookieService>();
        }

        [Fact]
        public async Task Set_valid_cookie_without_options_in_httpContext()
        {
            // arrange
            var sut = new CookieService(_contextAccessor, _logger);
            var key = "key123";
            var value = "value123";

            // act
            sut.SetCookie(new UserCookie { Key = key, Value = value });
            var actualValue = _contextAccessor.HttpContext!.Response.Headers.GetCookieTestValue();

            // assert
            Assert.Equal(value, actualValue);
        }

        [Fact]
        public async Task Set_valid_cookie_with_options_in_httpcontext()
        {
            // arrange
            var sut = new CookieService(_contextAccessor, _logger);
            var key = "key123";
            var value = "value123";
            var cookieOptions = new CookieOptions
            {
                Secure = true,
            };

            // act
            sut.SetCookie(new UserCookie { Key = key, Value = value, CookieOptions = cookieOptions });
            var actualValue = _contextAccessor.HttpContext!.Response.Headers.GetCookieTestValue();
            var actualOptions = _contextAccessor.HttpContext.Response.Headers.GetCookieTestOptions();

            // assert
            Assert.Equal(value, actualValue);
            Assert.Equal("secure", actualOptions);
        }

        [Theory]
        [MemberData(
            nameof(CookieServiceTestsDataGenerator.GenerateSetInvalidAuthData),
            MemberType = typeof(CookieServiceTestsDataGenerator))]
        public async Task Get_auth_user_data_from_httpContext_is_return_null_when_any_value_data_is_null(
            Claim[] claims,
            string accessToken,
            string refreshToken,
            string expireAt)
        {
            // arrange
            var sut = new CookieService(_contextAccessor, _logger);

            // setup user claims and authentication properties tokens in HttpContex
            var claimsIdentity = new ClaimsIdentity(claims, null);
            var principal = new ClaimsPrincipal(claimsIdentity);

            var authTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = accessToken },
                new AuthenticationToken { Name = OpenIdConnectParameterNames.RefreshToken, Value = refreshToken },
                new AuthenticationToken { Name = "expires_at", Value = expireAt }

            };

            var authProperties = new AuthenticationProperties();
            authProperties.StoreTokens(authTokens);

            var authenticateResult = AuthenticateResult
                .Success(new AuthenticationTicket(principal, authProperties, null));

            _mockAuthenticationService
                .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), null))
                .ReturnsAsync(authenticateResult);

            var mockService = new Mock<IServiceProvider>();
            mockService.Setup(sp => sp.GetService(typeof(IAuthenticationService))).Returns(_mockAuthenticationService.Object);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = principal,
                RequestServices = mockService.Object,
            });

            // act
            var result = await sut.GetOAuthUserData();

            // assert
            Assert.Null(result);
        }

        public async Task Get_auth_user_data_from_httpContext_when_value_data_is_not_null()
        {
            // arrange
            var sut = new CookieService(_contextAccessor, _logger);
            var name = "Name";
            var email = "test@gmail.com";
            var imageUri = "image.jpg";
            var accessToken = "Access_Token";
            var refreshToken = "Refresh_Token";
            var expireAt = DateTime.UtcNow.ToString();

            // setup user claims and authentication properties tokens in HttpContex
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name),
                new Claim("image", imageUri)
            };
            var claimsIdentity = new ClaimsIdentity(claims, null);
            var principal = new ClaimsPrincipal(claimsIdentity);

            var authTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = accessToken },
                new AuthenticationToken { Name = OpenIdConnectParameterNames.RefreshToken, Value = refreshToken },
                new AuthenticationToken { Name = "expires_at", Value = expireAt }

            };

            var authProperties = new AuthenticationProperties();
            authProperties.StoreTokens(authTokens);

            var authenticateResult = AuthenticateResult
                .Success(new AuthenticationTicket(principal, authProperties, null));

            _mockAuthenticationService
                .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), null))
                .ReturnsAsync(authenticateResult);

            var mockService = new Mock<IServiceProvider>();
            mockService.Setup(sp => sp.GetService(typeof(IAuthenticationService))).Returns(_mockAuthenticationService.Object);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = principal,
                RequestServices = mockService.Object,
            });

            // act
            var result = await sut.GetOAuthUserData();

            // assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Nickname);
            Assert.Equal(email, result.Email);
            Assert.Equal(imageUri, result.ImageUri);
            Assert.Equal(accessToken, result.AccessToken);
            Assert.Equal(refreshToken, result.RefreshToken);
            Assert.Equal(DateTime.SpecifyKind(DateTime.Parse(expireAt), DateTimeKind.Utc), result.ExpireAt);
        }

        [Fact]
        public async Task Delete_cookie_without_options_in_httpContext()
        {
            // arrange
            var sut = new CookieService(_contextAccessor, _logger);
            var key = "key123";
            var value = "value123";

            // act
            _contextAccessor.HttpContext!.Response.Cookies.Append(key, value);
            sut.DeleteCookie(key);
            var actualValue = _contextAccessor.HttpContext.Response.Headers.GetCookieTestValue();

            // assert
            Assert.Equal(string.Empty, actualValue);
        }

        [Fact]
        public async Task Delete_cookie_with_options_in_httpcontext()
        {
            // arrange
            var sut = new CookieService(_contextAccessor, _logger);
            var key = "key123";
            var value = "value123";
            var cookieOptions = new CookieOptions
            {
                Secure = true,
            };

            // act
            _contextAccessor.HttpContext!.Response.Cookies.Append(key, value, cookieOptions);
            sut.DeleteCookie(key, cookieOptions);
            var actualValue = _contextAccessor.HttpContext.Response.Headers.GetCookieTestValue();

            // assert
            Assert.Equal(string.Empty, actualValue);
        }
    }
}
