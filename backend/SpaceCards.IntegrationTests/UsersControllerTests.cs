using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json.Linq;
using System.Net;

namespace SpaceCards.IntegrationTests
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
            var schema = "Bearer";
            var token = await Client.GetAsync("users/token");
            var tokenJson = await token.Content.ReadAsStringAsync();
            var tokenString = JToken.Parse(tokenJson).ToString();
            Client.DefaultRequestHeaders.Add("Authorization", $"{schema} " + tokenString);

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
