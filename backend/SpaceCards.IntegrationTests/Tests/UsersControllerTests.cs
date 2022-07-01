using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using SpaceCards.API;

namespace SpaceCards.IntegrationTests.Tests
{
    public class UsersControllerTests : BaseControllerTests
    {
        public UsersControllerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task Validate_ShouldReturnOk()
        {
            // arrange
            var token = await Client.GetAsync("users/token");
            var tokenJson = await token.Content.ReadAsStringAsync();
            var tokenString = JToken.Parse(tokenJson).ToString();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                BaseSchema.NAME,
                tokenString);

            // act
            var response = await Client.GetAsync($"users/validate?token={tokenString}");

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Validate_ShouldReturnUnauthorized()
        {
            // arrange
            var token = await Client.GetAsync("users/token");
            var tokenJson = await token.Content.ReadAsStringAsync();
            var tokenString = JToken.Parse(tokenJson).ToString();

            // act
            var response = await Client.GetAsync($"users/validate?token={tokenString}");

            // assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
