using AutoFixture;
using SpaceCards.API.Contracts;
using SpaceCards.IntegrationTests.MemberData;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests.Tests
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
            await SignIn();

            // act
            var response = await Client.GetAsync("cards");

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            // arrange
            await SignIn();
            var card = Fixture.Create<CreateCardRequest>();

            // act
            var response = await Client.PostAsJsonAsync("cards", card);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetInvalidFrontSideBackSide),
            parameters: 5,
            MemberType = typeof(CardDataGenerator))]
        public async Task Create_ShouldReturnBadRequest(string frontSide, string backSide)
        {
            // arrange
            var card = Fixture.Build<CreateCardRequest>()
                .With(x => x.FrontSide, frontSide)
                .With(x => x.BackSide, backSide)
                .Create();

            var errors = Fixture.CreateMany<string>().ToArray();

            // act
            var response = await Client.PostAsJsonAsync("cards", card);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            // arrange
            var cardId = await MakeCard();

            var updatedCard = Fixture.Create<UpdateCardRequest>();

            // act
            var response = await Client.PutAsJsonAsync($"cards/{cardId}", updatedCard);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetInvalidCardIdFrontSideBackSide),
            parameters: 5,
            MemberType = typeof(CardDataGenerator))]
        public async Task Update_ShouldReturnBadRequest(int cardId, string frontSide, string backSide)
        {
            // arrange
            var updatedCard = Fixture.Build<UpdateCardRequest>()
                .With(x => x.FrontSide, frontSide)
                .With(x => x.BackSide, backSide)
                .Create();

            var errors = Fixture.CreateMany<string>().ToArray();

            // act
            var response = await Client.PutAsJsonAsync($"cards/{cardId}", updatedCard);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            // arrange
            var cardId = await MakeCard();

            // act
            var response = await Client.DeleteAsync($"cards/{cardId}");

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetInvalidCardId),
            parameters: 5,
            MemberType = typeof(CardDataGenerator))]
        public async Task Delete_ShouldReturnBadRequest(int invalidCardId)
        {
            // arrange
            var cardId = await MakeCard();

            // act
            var response = await Client.DeleteAsync($"cards/{invalidCardId}");

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
