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
        public async Task SaveGuessedCard_ShouldReturnOk()
        {
            // arrange
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={cardId}", groupId);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(GuessedCardsDataGenerator.GenerateSetInvalidCardIdGroupId),
            parameters: 5,
            MemberType = typeof(GuessedCardsDataGenerator))]
        public async Task SaveGuessedCard_CardIdAndGroupIdInvalid_ShouldReturnBadRequest(
            int invalidCardId,
            int invalidGroupId)
        {
            // arrange
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={invalidCardId}", invalidGroupId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [MemberData(
            nameof(GuessedCardsDataGenerator.GenerateSetInvalidCardIdOrGroupId),
            parameters: 5,
            MemberType = typeof(GuessedCardsDataGenerator))]
        public async Task SaveGuessedCard_CardIdInvalid_ShouldReturnBadRequest(int invalidCardId)
        {
            // arrange
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={invalidCardId}", groupId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [MemberData(
            nameof(GuessedCardsDataGenerator.GenerateSetInvalidCardIdOrGroupId),
            parameters: 5,
            MemberType = typeof(GuessedCardsDataGenerator))]
        public async Task SaveGuessedCard_GroupIdInvalid_ShouldReturnBadRequest(int invalidGroupId)
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
