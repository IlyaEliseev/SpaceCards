using AutoFixture;
using SpaceCards.Domain;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests
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

            // act
            var (card, errors) = Card.Create(frontSide, backSide);

            // assert
            Assert.NotNull(card);
            Assert.Empty(errors);
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetInvalidFrontSideOrBackSide),
            parameters: 5,
            MemberType = typeof(CardDataGenerator))]
        public async Task Create_FrontSideIsNotValidNullOrWhitespace_ShouldReturnNullAndError(string frontSide)
        {
            // arrange
            var backSide = _fixture.Create<string>();
            var excpectedError = $"'{nameof(frontSide)}' cannot be null or whitespace.";

            // act
            var (card, errors) = Card.Create(frontSide, backSide);

            // assert
            var error = errors.FirstOrDefault();
            Assert.Null(card);
            Assert.NotEmpty(errors);
            Assert.Equal(excpectedError, error);
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetInvalidFrontSideOrBackSide),
            parameters: 5,
            MemberType = typeof(CardDataGenerator))]
        public async Task Create_BackSideIsNotValidNullOrWhitespace_ShouldReturnNullAndError(string backSide)
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var excpectedError = $"'{nameof(backSide)}' cannot be null or whitespace.";

            // act
            var (card, errors) = Card.Create(frontSide, backSide);

            // assert
            var error = errors.FirstOrDefault();
            Assert.Null(card);
            Assert.NotEmpty(errors);
            Assert.Equal(excpectedError, error);
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetBackSideWithInvalidLength),
            parameters: 5,
            MemberType = typeof(CardDataGenerator))]
        public async Task Create_FrontSideIsNotValidMoreThanMaxLength_ShouldReturnNullAndError(string frontSide)
        {
            // arrange
            var backSide = _fixture.Create<string>();
            var excpectedError = $"'{nameof(frontSide)}' more than {Card.MAX_NAME_FRONTSIDE} characters.";

            // act
            var (card, errors) = Card.Create(frontSide, backSide);

            // assert
            var error = errors.FirstOrDefault();
            Assert.Null(card);
            Assert.NotEmpty(errors);
            Assert.Equal(excpectedError, error);
        }

        [Theory]
        [MemberData(
            nameof(CardDataGenerator.GenerateSetBackSideWithInvalidLength),
            parameters: 5,
            MemberType = typeof(CardDataGenerator))]
        public async Task Create_BackSideIsNotValidMoreThanMaxLength_ShouldReturnNullAndError(string backSide)
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var excpectedError = $"'{nameof(backSide)}' more than {Card.MAX_NAME_BACKSIDE} characters.";

            // act
            var (card, errors) = Card.Create(frontSide, backSide);

            // assert
            var error = errors.FirstOrDefault();
            Assert.Null(card);
            Assert.NotEmpty(errors);
            Assert.Equal(excpectedError, error);
        }
    }
}
