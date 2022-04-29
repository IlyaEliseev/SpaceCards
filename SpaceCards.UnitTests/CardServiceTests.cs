using AutoFixture;
using Moq;
using SpaceCards.BusinessLogic;
using SpaceCards.Domain;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests
{
    public class CardServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICardsRepository> _cardsRepositoryMock;
        private readonly CardsService _service;

        public CardServiceTests()
        {
            _fixture = new Fixture();
            _cardsRepositoryMock = new Mock<ICardsRepository>();
            _service = new CardsService(_cardsRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_CardIsValid_ShouldCreateNewCard()
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var backSide = _fixture.Create<string>();
            var expectedCardId = _fixture.Create<int>();

            var (card, modelErrors) = Card.Create(frontSide, backSide);

            _cardsRepositoryMock.Setup(x => x.Add(card))
                .ReturnsAsync(expectedCardId);

            // act
            var (result, errors) = await _service.Create(frontSide, backSide);

            // assert
            Assert.NotNull(card);
            Assert.Empty(errors);
            Assert.Equal(expectedCardId, result);
            _cardsRepositoryMock.Verify(x => x.Add(card), Times.Once);
        }

        [Theory]
        [InlineData(null, "Test")]
        [InlineData("", "Test")]
        [InlineData(" ", "Test")]
        [InlineData("  ", "Test")]
        public async Task Create_CardIsNotValid_ShouldReturnErrorMessage_FrontSideIsNullOrWhitespace(string frontSide, string backSide)
        {
            // arrange
            var errorMessage = $"'{nameof(frontSide)}' cannot be null or whitespace.";

            // act
            var (result, errors) = await _service.Create(frontSide, backSide);
            var expectedError = errors.FirstOrDefault();

            // assert
            Assert.Equal(0, result);
            Assert.Equal(expectedError, errorMessage);
        }

        [Theory]
        [InlineData("Test", null)]
        [InlineData("Test", "")]
        [InlineData("Test", " ")]
        [InlineData("Test", "  ")]
        public async Task Create_CardIsNotValid_ShouldReturnErrorMessage_BackSideIsNullOrWhitespace(string frontSide, string backSide)
        {
            // arrange
            var errorMessage = $"'{nameof(backSide)}' cannot be null or whitespace.";

            // act
            var (result, errors) = await _service.Create(frontSide, backSide);
            var expectedError = errors.FirstOrDefault();

            // assert
            Assert.Equal(0, result);
            Assert.Equal(expectedError, errorMessage);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public async Task Create_CardIsNotValid_ShouldReturnErrorMessage_FrontSideBackSideIsNullOrWhitespace(string frontSide, string backSide)
        {
            // arrange

            // act
            var (result, errors) = await _service.Create(frontSide, backSide);
            var errorsCount = errors.Count();

            // assert
            Assert.Equal(0, result);
            Assert.Equal(2, errorsCount);
        }

        [Fact]
        public async Task Get_ShouldGetIsValid_ReturnCards()
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var backSide = _fixture.Create<string>();
            var (card, modelErrors) = Card.Create(frontSide, backSide);

            var expectedCards = Enumerable.Range(1, 5).Select(x => card).ToArray();

            _cardsRepositoryMock.Setup(x => x.Get()).ReturnsAsync(expectedCards);

            // act
            var cards = await _service.Get();

            // assert
            Assert.NotNull(cards);
            Assert.Equal(expectedCards, cards);
            _cardsRepositoryMock.Verify(x => x.Get(), Times.Once);
        }

        [Fact]
        public async Task Delete_DeleteIsValid_ShouldReturnTrue()
        {
            // arrange
            var cardId = _fixture.Create<int>();
            var frontSide = _fixture.Create<string>();
            var backSide = _fixture.Create<string>();

            var (card, modelErrors) = Card.Create(frontSide, backSide);

            _cardsRepositoryMock.Setup(x => x.GetById(cardId))
                .ReturnsAsync(card);

            // act
            var (result, errors) = await _service.Delete(cardId);

            // assert
            Assert.True(result);
            _cardsRepositoryMock.Verify(x => x.Delete(cardId), Times.Once);
        }

        [Theory]
        [InlineData(default)]
        [InlineData(-1)]
        [InlineData(-112)]
        public async Task Delete_CardIdIsNotValid_ShouldReturnFalse(int cardId)
        {
            // arrange

            // act
            var (result, errors) = await _service.Delete(cardId);

            // assert
            Assert.False(result);
            _cardsRepositoryMock.Verify(x => x.Delete(cardId), Times.Never);
        }

        [Fact]
        public async Task Update_CardIdFrontSideBackSideIsValid_ShouldReturnTrue()
        {
            // arrange
            var cardId = _fixture.Create<int>();
            var frontSide = _fixture.Create<string>();
            var backSide = _fixture.Create<string>();
            var newFrontSide = _fixture.Create<string>();
            var newBackSide = _fixture.Create<string>();

            var (card, modelErrors) = Card.Create(frontSide, backSide);

            _cardsRepositoryMock.Setup(x => x.GetById(cardId))
                .ReturnsAsync(card);

            // act
            var (result, errors) = await _service.Update(cardId, newFrontSide, newBackSide);

            // assert
            Assert.True(result);
            _cardsRepositoryMock.Verify(x => x.GetById(cardId), Times.Once);
        }

        [Theory]
        [InlineData(default, null, null)]
        [InlineData(default, "", "")]
        [InlineData(default, " ", " ")]
        [InlineData(default, "   ", "   ")]
        [InlineData(default, null, "")]
        [InlineData(default, null, " ")]
        [InlineData(default, null, "   ")]
        [InlineData(default, "", null)]
        [InlineData(default, " ", null)]
        [InlineData(default, "   ", null)]
        public async Task Update_CardIdFrontSideBackSideIsNotValid_ShouldReturnFalse(int cardId, string frontSide, string backSide)
        {
            // arrange
            var (card, modelErrors) = Card.Create(frontSide, backSide);

            // act
            var (result, errors) = await _service.Update(cardId, frontSide, backSide);
            var errorsCount = errors.Count();

            // assert
            Assert.False(result);
            _cardsRepositoryMock.Verify(x => x.Update(card), Times.Never);
        }
    }
}
