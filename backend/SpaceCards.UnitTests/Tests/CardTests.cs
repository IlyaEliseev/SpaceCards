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
        public async Task Create_IsValid_ShouldReturnCard()
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var backSide = _fixture.Create<string>();
            var userId = _fixture.Create<Guid>();

            // act
            var (card, errors) = Card.Create(frontSide, backSide, userId);

            // assert
            Assert.NotNull(card);
            Assert.Empty(errors);
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetInvalidString),
            parameters: 10,
            MemberType = typeof(CardDataGenerator))]
        public async Task Create_FrontSideInvalid_ShouldReturnNullAndError(string frontSide)
        {
            // arrange
            var backSide = _fixture.Create<string>();
            var userId = _fixture.Create<Guid>();

            // act
            var (card, errors) = Card.Create(frontSide, backSide, userId);

            // assert
            Assert.Null(card);
            Assert.NotEmpty(errors);
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetInvalidString),
            parameters: 10,
            MemberType = typeof(CardDataGenerator))]
        public async Task Create_BackSideInvalid_ShouldReturnNullAndError(string backSide)
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var userId = _fixture.Create<Guid>();

            // act
            var (card, errors) = Card.Create(frontSide, backSide, userId);

            // assert
            Assert.Null(card);
            Assert.NotEmpty(errors);
        }
    }
}
