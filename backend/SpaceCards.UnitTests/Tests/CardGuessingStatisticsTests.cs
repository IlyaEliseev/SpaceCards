using AutoFixture;
using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.MemberData;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests.Tests
{
    public class CardGuessingStatisticsTests
    {
        private readonly Fixture _fixture;
        private readonly Random _rnd;

        public CardGuessingStatisticsTests()
        {
            _fixture = new Fixture();
            _rnd = new Random();
        }

        [Fact]
        public async Task Creat_CardGuessingStatistics_with_a_valid_parameters_is_not_failure()
        {
            // arrange
            var cardId = _fixture.Create<int>();
            var success = _rnd.Next(0, 2);
            var userId = _fixture.Create<Guid>();

            // act
            var result = CardGuessingStatistics.Create(cardId, success, userId);

            // assert
            Assert.NotNull(result.Value);
            Assert.Equal(success, result.Value.Success);
            Assert.Equal(cardId, result.Value.CardId);
            Assert.Equal(userId, result.Value.UserId);
        }

        [Theory]
        [MemberData(
            nameof(CardGuessingStatisticsDataGenerator
            .GenerateSetInvalidCardIdSuccessUserId),
            parameters: 10,
            MemberType = typeof(CardGuessingStatisticsDataGenerator))]
        public async Task Creat_CardGuessingStatistics_with_a_not_valid_parameters_is_failure(
            int cardId,
            int success,
            Guid userId)
        {
            // arrange
            // act
            var result = CardGuessingStatistics.Create(cardId, success, userId);

            // assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error);
        }
    }
}
