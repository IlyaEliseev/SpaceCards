using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using SpaceCards.API.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.IntegrationTests
{
    public class CardsControllerTests
    {
        private readonly HttpClient _client;
        private readonly Fixture _fixture;

        public CardsControllerTests()
        {
            var app = new WebApplicationFactory<Program>();
            _client = app.CreateClient();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_ShouldReturnOk()
        {
            // arrange

            // act
            var response = await _client.GetAsync("cards");

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            // arrange
            var card = _fixture.Create<CreateCardRequest>();

            // act
            var response = await _client.PostAsJsonAsync("cards", card);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("   ", "   ")]
        [InlineData(null, "")]
        [InlineData(null, " ")]
        [InlineData(null, "   ")]
        [InlineData("", null)]
        [InlineData(" ", null)]
        [InlineData("   ", null)]
        public async Task Create_ShouldReturnBadRequest(string frontSide, string backSide)
        {
            // arrange
            var card = _fixture.Build<CreateCardRequest>()
                .With(x => x.FrontSide, frontSide)
                .With(x => x.BackSide, backSide)
                .Create();

            var errors = _fixture.CreateMany<string>().ToArray();

            // act
            var response = await _client.PostAsJsonAsync("cards", card);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            // arrange
            var cardId = _fixture.Create<int>();
            var card = _fixture.Create<UpdateCardRequest>();

            // act
            var response = await _client.PutAsJsonAsync($"cards/{2}", card);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(default, null, null)]
        [InlineData(default, "", "")]
        [InlineData(default, " ", " ")]
        [InlineData(default, "   ", "   ")]
        [InlineData(default, null, "")]
        [InlineData(default, null, " ")]
        [InlineData(default, null, "   ")]
        [InlineData(default, "", null)]
        [InlineData(default, " ", null)]
        [InlineData(default, "   ", null)]
        public async Task Update_ShouldReturnBadRequest(int cardId, string frontSide, string backSide)
        {
            // arrange
            var card = _fixture.Build<UpdateCardRequest>()
                .With(x => x.FrontSide, frontSide)
                .With(x => x.BackSide, backSide)
                .Create();

            var errors = _fixture.CreateMany<string>().ToArray();

            // act
            var response = await _client.PutAsJsonAsync($"cards/{cardId}", card);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            // arrange
            var cardId = _fixture.Create<int>();

            // act
            var response = await _client.DeleteAsync($"cards/{cardId}");

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(default)]
        [InlineData(-1)]
        [InlineData(-111)]
        public async Task Delete_ShouldReturnBadRequest(int cardId)
        {
            // arrange

            // act
            var response = await _client.DeleteAsync($"cards/{cardId}");

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
