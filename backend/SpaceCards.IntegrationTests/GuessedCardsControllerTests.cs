using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests
{
    public class GuessedCardsControllerTests : BaseControllerTests
    {
        public GuessedCardsControllerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task TakeGuessedCard_ShouldReturnOk()
        {
            // arrange
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={cardId}", groupId);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-111, -111)]
        [InlineData(0, 0)]
        public async Task TakeGuessedCard_CardIdAndGroupIdInvalid_ShouldReturnBadRequest(int cardId, int groupId)
        {
            // arrange
            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={cardId}", groupId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-111)]
        [InlineData(0)]
        public async Task TakeGuessedCard_CardIdInvalid_ShouldReturnBadRequest(int invalidCardId)
        {
            // arrange
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={invalidCardId}", groupId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-111)]
        [InlineData(0)]
        public async Task TakeGuessedCard_GroupIdInvalid_ShouldReturnBadRequest(int invalidGroupId)
        {
            // arrange
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={cardId}", invalidGroupId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
