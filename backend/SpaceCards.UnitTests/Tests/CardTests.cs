using AutoFixture;
using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.MemberData;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests.Tests
{
    public class CardTests
    {
        private readonly Fixture _fixture;
        private readonly StringFixture _stringFixture;

        public CardTests()
        {
            _fixture = new Fixture();
            _stringFixture = new StringFixture();
        }

        [Fact]
        public async Task Card_with_valid_parameters_is_creating()
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var backSide = _fixture.Create<string>();
            var userId = _fixture.Create<Guid>();

            // act
            var result = Card.Create(frontSide, backSide, userId);

            // assert
            Assert.NotNull(result.Value);
            Assert.Equal(frontSide, result.Value.FrontSide);
            Assert.Equal(backSide, result.Value.BackSide);
            Assert.False(result.IsFailure);
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetInvalidFrontside),
            parameters: 10,
            MemberType = typeof(CardDataGenerator))]
        public async Task Card_with_invalid_frontside_is_not_creating(string frontSide)
        {
            // arrange
            var backSide = _fixture.Create<string>();
            var userId = _fixture.Create<Guid>();

            // act
            var result = Card.Create(frontSide, backSide, userId);

            // assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error);
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetInvalidBackside),
            parameters: 10,
            MemberType = typeof(CardDataGenerator))]
        public async Task Card_with_invalid_backside_is_not_creating(string backSide)
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var userId = _fixture.Create<Guid>();

            // act
            var result = Card.Create(frontSide, backSide, userId);

            // assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error);
        }
    }
}
