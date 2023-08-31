using SpaceCards.IntegrationTests.MemberData;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests.Tests
{
    public class StatisticsControllerTests : BaseControllerTests
    {
        public StatisticsControllerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task Collect_success_guessing_card_one_or_zero_with_valid_card_id_and_success_is_response_Ok()
        {
            // arrange
            await SignIn();
            var rnd = new Random();
            var cardId = await MakeCard();
            var success = rnd.Next(0, 2);

            // act
            var response = await Client.PostAsync($"statistics/{cardId}?successValue={success}", null);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(CardGuessingStatisticsDataGenerator.GenerateSetinvalidCardIdSuccess),
            MemberType = typeof(CardGuessingStatisticsDataGenerator))]
        public async Task Collect_success_guessing_card_one_or_zero_with_invalid_card_id_and_success_is_response_BadRequest(
            int cardId,
            int success)
        {
            // arrange
            await SignIn();
            await MakeCard();

            // act
            var response = await Client.PostAsync(
                $"statistics/{cardId}?successValue={success}", null);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_all_row_cards_with_success_one_or_zero_is_response_Ok()
        {
            // arrange
            await SignIn();
            await MakeCardGuessingStatistics();

            // act
            var response = await Client.GetAsync($"statistics");

            // assert
            response.EnsureSuccessStatusCode();
        }
    }
}
