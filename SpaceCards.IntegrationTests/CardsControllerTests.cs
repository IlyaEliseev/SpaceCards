using AutoFixture;
using SpaceCards.API.Contracts;
using SpaceCards.Domain;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests
{
    public class CardsControllerTests : BaseControllerTests
    {
        public CardsControllerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
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
            var frontSide = _fixture.Create<string>();
            var backSide = _fixture.Create<string>();
            var (card, errors) = Card.Create(frontSide, backSide);
            var cardId = await _cardRepository.Add(card);

            var updatedCard = _fixture.Create<UpdateCardRequest>();

            // act
            var response = await _client.PutAsJsonAsync($"cards/{cardId}", updatedCard);

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
            var updatedCard = _fixture.Build<UpdateCardRequest>()
                .With(x => x.FrontSide, frontSide)
                .With(x => x.BackSide, backSide)
                .Create();

            var errors = _fixture.CreateMany<string>().ToArray();

            // act
            var response = await _client.PutAsJsonAsync($"cards/{cardId}", updatedCard);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var backSide = _fixture.Create<string>();
            var (card, errors) = Card.Create(frontSide, backSide);
            var cardId = await _cardRepository.Add(card);

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
