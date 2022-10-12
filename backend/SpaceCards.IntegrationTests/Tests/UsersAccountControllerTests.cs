using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Net;
using SpaceCards.API.Contracts;
using System.Net.Http.Json;
using SpaceCards.IntegrationTests.MemberData;

namespace SpaceCards.IntegrationTests.Tests
{
    public class UsersAccountControllerTests : BaseControllerTests
    {
        public UsersAccountControllerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task Registration_ShouldReturnOk()
        {
            // arrange
            var user = new UserRegistrationRequest
            {
                Nickname = UserNickname,
                Email = UserEmail,
                Password = UserPassword
            };

            // act
            var response = await Client.PostAsJsonAsync("usersaccount/registration", user);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(UsersAccountDataGenerator.GenerateSetInvalidEmail),
            parameters: 10,
            MemberType = typeof(UsersAccountDataGenerator))]
        public async Task RegistrationUser_InvalidEmail_ShouldReturnBadRequest(
            string email)
        {
            var registrationRequest = new UserRegistrationRequest
            {
                Nickname = UserNickname,
                Email = email,
                Password = UserPassword
            };

            // act
            var response = await Client.PostAsJsonAsync("usersaccount/registration", registrationRequest);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [MemberData(
            nameof(UsersAccountDataGenerator.GenerateSetInvalidPassword),
            parameters: 10,
            MemberType = typeof(UsersAccountDataGenerator))]
        public async Task RegistrationUser_InvalidPassword_ShouldReturnBadRequest(
            string password)
        {
            var registrationRequest = new UserRegistrationRequest
            {
                Email = UserEmail,
                Password = password,
                Nickname = UserNickname
            };

            // act
            var response = await Client.PostAsJsonAsync("usersaccount/registration", registrationRequest);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RegistrationUser_UserAlreadyExist_ShouldReturnBadRequest()
        {
            // arrange
            var registrationRequest = new UserRegistrationRequest
            {
                Email = UserEmail,
                Password = UserPassword,
                Nickname = UserNickname
            };

            // act
            await Client.PostAsJsonAsync("usersaccount/registration", registrationRequest);
            var response = await Client.PostAsJsonAsync("usersaccount/registration", registrationRequest);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturnOk()
        {
            // arrange
            var registrationRequest = new UserRegistrationRequest
            {
                Email = UserEmail,
                Password = UserPassword,
                Nickname = UserNickname
            };

            var loginRequest = new LoginRequest
            {
                Email = UserEmail,
                Password = UserPassword
            };

            // act
            await Client.PostAsJsonAsync("usersaccount/registration", registrationRequest);
            var response = await Client.PostAsJsonAsync("usersaccount/login", loginRequest);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Login_SessionIsAlreadyExist_ShouldReturnOk()
        {
            // arrange
            var registrationRequest = new UserRegistrationRequest
            {
                Email = UserEmail,
                Password = UserPassword,
                Nickname = UserNickname
            };

            var loginRequest = new LoginRequest
            {
                Email = UserEmail,
                Password = UserPassword
            };

            // act
            await Client.PostAsJsonAsync("usersaccount/registration", registrationRequest);
            await Client.PostAsJsonAsync("usersaccount/login", loginRequest);
            var response = await Client.PostAsJsonAsync("usersaccount/login", loginRequest);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest()
        {
            // arrange
            var registrationRequest = new UserRegistrationRequest
            {
                Email = UserEmail,
                Password = UserPassword,
                Nickname = UserNickname
            };

            var loginRequest = new LoginRequest
            {
                Email = "11111@gmail.com",
                Password = "12345"
            };

            // act
            await Client.PostAsJsonAsync("usersaccount/registration", registrationRequest);
            var response = await Client.PostAsJsonAsync("usersaccount/login", loginRequest);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnOk()
        {
            // arrange
            var (accessToken, refreshToken) = await MakeSession();

            var refreshRequest = new TokenRequest
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            // act
            var response = await Client.PostAsJsonAsync("usersaccount/refreshaccesstoken", refreshRequest);

            // assert
            response.EnsureSuccessStatusCode();
        }
    }
}
