using SpaceCards.IntegrationTests.MemberData;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests.Tests
{
    public class GuessedCardsControllerTests : BaseControllerTests
    {
        public GuessedCardsControllerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task Collect_Guessed_card_is_response_Ok()
        {
            // arrange
            await SignIn();
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={cardId}", groupId);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(GuessedCardsDataGenerator.GenerateSetInvalidCardIdGroupId),
            MemberType = typeof(GuessedCardsDataGenerator))]
        public async Task Collect_Guessed_card_with_invalid_card_id_and_group_id_is_response_BadRequest(
            int invalidCardId,
            int invalidGroupId)
        {
            // arrange
            await SignIn();
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={invalidCardId}", invalidGroupId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [MemberData(
            nameof(GuessedCardsDataGenerator.GenerateSetInvalidId),
            MemberType = typeof(GuessedCardsDataGenerator))]
        public async Task Collect_Guessed_card_with_invalid_card_id_and_valid_group_id_is_response_BadRequest(
            int invalidCardId)
        {
            // arrange
            await SignIn();
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={invalidCardId}", groupId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [MemberData(
            nameof(GuessedCardsDataGenerator.GenerateSetInvalidId),
            MemberType = typeof(GuessedCardsDataGenerator))]
        public async Task Collect_Guessed_card_with_valid_card_id_and_invalid_group_id_is_response_BadRequest(
            int invalidGroupId)
        {
            // arrange
            await SignIn();
            var (groupId, cardId) = await AddCardInGroup();

            // act
            var response = await Client.PostAsJsonAsync($"guessedCards?cardId={cardId}", invalidGroupId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
