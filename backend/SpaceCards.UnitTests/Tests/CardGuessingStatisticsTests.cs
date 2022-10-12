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
        public async Task Create_IsValid_ShouldReturnCardGuessingStatistics()
        {
            // arrange
            var cardId = _fixture.Create<int>();
            var success = _rnd.Next(0, 2);
            var userId = _fixture.Create<Guid>();

            // act
            var (result, errors) = CardGuessingStatistics.Create(cardId, success, userId);

            // assert
            Assert.NotNull(result);
            Assert.Empty(errors);
        }

        [Theory]
        [MemberData(
            nameof(CardGuessingStatisticsDataGenerator
            .GenerateSetInvalidCardIdSuccessUserId),
            parameters: 20,
            MemberType = typeof(CardGuessingStatisticsDataGenerator))]
        public async Task Create_CardIdIsInvalid_ShouldReturnNullAndError(
            int cardId,
            int success,
            Guid userId)
        {
            // arrange
            // act
            var (result, errors) = CardGuessingStatistics.Create(cardId, success, userId);

            // assert
            Assert.Null(result);
            Assert.NotEmpty(errors);
        }
    }
}
