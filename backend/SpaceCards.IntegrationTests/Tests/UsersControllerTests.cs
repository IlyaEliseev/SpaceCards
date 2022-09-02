using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Net;
using SpaceCards.API.Contracts;
using System.Net.Http.Json;

namespace SpaceCards.IntegrationTests.Tests
{
    public class UsersControllerTests : BaseControllerTests
    {
        public UsersControllerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task Registration_ShouldReturnOk()
        {
            // arrange
            var email = "sdfkljsdvnsd@mail.ru";
            var password = "Sdvs5&dvsdasev";

            var user = new UserRegistrationRequest
            {
                Email = email,
                Password = password
            };

            // act
            var response = await Client.PostAsJsonAsync("usersaccount/registration", user);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Registreation_ShouldReturnBadRequest()
        {
            // arrange
            var email = "sdfkljsdvnsd@mail";
            var password = "dvdvsdasev";

            var user = new UserRegistrationRequest
            {
                Email = email,
                Password = password
            };

            // act
            var response = await Client.PostAsJsonAsync("usersaccount/registration", user);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
