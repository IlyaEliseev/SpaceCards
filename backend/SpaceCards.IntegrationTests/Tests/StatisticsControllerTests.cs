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
        public async Task CollectCardStatistics_ShouldReturnOk()
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
            parameters: 10,
            MemberType = typeof(CardGuessingStatisticsDataGenerator))]
        public async Task CollectCardStatistic_ShouldReturnBadRequest(int cardId, int success)
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
        public async Task GetGetCardGuessingStatistics_ShouldReturnOk()
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
