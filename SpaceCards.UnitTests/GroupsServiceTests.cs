using AutoFixture;
using Moq;
using SpaceCards.BusinessLogic;
using SpaceCards.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests
{
    public class GroupsServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IGroupsRepository> _groupsRepositoryMock;
        private readonly Mock<ICardsRepository> _cardsRepositoryMock;
        private readonly GroupsService _service;

        public GroupsServiceTests()
        {
            _fixture = new Fixture();
            _groupsRepositoryMock = new Mock<IGroupsRepository>();
            _cardsRepositoryMock = new Mock<ICardsRepository>();
            _service = new GroupsService(_groupsRepositoryMock.Object, _cardsRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_GroupIsValid_ShouldCreateNewGroup()
        {
            // arrange
            var name = _fixture.Create<string>();
            var expectedGroupId = _fixture.Create<int>();

            var (group, modelErrors) = Group.Create(name);

            _groupsRepositoryMock.Setup(x => x.Add(group))
                .ReturnsAsync((expectedGroupId, Array.Empty<string>()));

            // act
            var (result, errors) = await _service.Create(name);

            // assert
            Assert.NotNull(result);
            Assert.Empty(errors);
            Assert.Equal(expectedGroupId, result);
            _groupsRepositoryMock.Verify(x => x.Add(group), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        public async Task Create_GroupIsNotValid_ShouldReturnErrorMessage_NameIsNullOrWhitespace(string name)
        {
            // arrange
            var errorMessage = $"'{nameof(name)}' cannot be null or whitespace.";

            // act
            var (result, errors) = await _service.Create(name);
            var expectedError = errors.FirstOrDefault();

            // assert
            Assert.Equal(0, result);
            Assert.Equal(expectedError, errorMessage);
        }

        [Fact]
        public async Task Get_GetIsValid_ShouldReturnGroups()
        {
            // arrange
            var name = _fixture.Create<string>();
            var (group, modelErrors) = Group.Create(name);

            var expectedGroups = Enumerable.Range(1, 5)
                .Select(x => group)
                .ToArray();

            _groupsRepositoryMock.Setup(x => x.Get()).ReturnsAsync(expectedGroups);

            // act
            var groups = await _service.Get();

            // assert
            Assert.NotNull(groups);
            Assert.Equal(expectedGroups, groups);
            _groupsRepositoryMock.Verify(x => x.Get(), Times.Once);
        }

        [Fact]
        public async Task Delete_DeleteIsValid_ShouldReturnTrue()
        {
            // arrange
            var groupId = _fixture.Create<int>();
            var name = _fixture.Create<string>();
            var (group, modelErrors) = Group.Create(name);

            _groupsRepositoryMock.Setup(x => x.GetById(groupId))
                .ReturnsAsync(group);

            // act
            var (result, errors) = await _service.Delete(groupId);
            var groups = await _service.Get();

            // assert
            Assert.True(result);
            Assert.Empty(groups);
            _groupsRepositoryMock.Verify(x => x.Delete(groupId), Times.Once);
        }

        [Theory]
        [InlineData(default)]
        [InlineData(-1)]
        [InlineData(-112)]
        public async Task Delete_GroupIdIsNotValid_ShouldReturnFalse(int groupId)
        {
            // arrange

            // act
            var (result, errors) = await _service.Delete(groupId);

            // assert
            Assert.False(result);
            _groupsRepositoryMock.Verify(x => x.Delete(groupId), Times.Never);
        }

        [Fact]
        public async Task Delete_CardsIsNotEmpty_ShouldReturnFalse()
        {
            // arrange
            var frontSide = _fixture.Create<string>();
            var backtSide = _fixture.Create<string>();
            var groupName = _fixture.Create<string>();

            var (group, groupErrors) = Group.Create(groupName);
            var (card, cardErrors) = Card.Create(frontSide, backtSide);

            var groupWhithCards = group with { Cards = new[] { card } };

            // act
            var (result, errors) = await _service.Delete(groupWhithCards.Id);

            // assert
            Assert.False(result);
            Assert.NotEmpty(errors);
        }

        [Fact]
        public async Task Update_GroupIdNameIsValid_ShouldReturnTrue()
        {
            // arrange
            var name = _fixture.Create<string>();
            var groupId = _fixture.Create<int>();
            var newName = _fixture.Create<string>();
            var (group, modelErrors) = Group.Create(name);

            _groupsRepositoryMock.Setup(x => x.GetById(groupId))
                .ReturnsAsync(group);

            // act
            var (result, errors) = await _service.Update(groupId, newName);

            // assert
            Assert.True(result);
            _groupsRepositoryMock.Verify(x => x.GetById(groupId), Times.Once);
        }

        [Theory]
        [InlineData(default, null)]
        [InlineData(default, "")]
        [InlineData(default, " ")]
        [InlineData(default, "   ")]
        public async Task Update_GroupIdNameIsNotValid_ShouldReturnFaulse(int groupId, string name)
        {
            // arrange
            var (group, modelErrors) = Group.Create(name);

            _groupsRepositoryMock.Setup(x => x.GetById(groupId))
                .ReturnsAsync(default(Group));

            // act
            var (result, errors) = await _service.Update(groupId, name);
            var errorsCount = errors.Count();

            // assert
            Assert.False(result);
            _groupsRepositoryMock.Verify(x => x.Update(group), Times.Never);
        }

        [Fact]
        public async Task AddCard_CardIdGroupIdIsValid_ShouldReturnTrue()
        {
            // arrange
            var cardId = _fixture.Create<int>();
            var groupId = _fixture.Create<int>();
            var frontSide = _fixture.Create<string>();
            var backtSide = _fixture.Create<string>();
            var groupName = _fixture.Create<string>();

            var (card, cardErrors) = Card.Create(frontSide, backtSide);
            var (group, groupErrors) = Group.Create(groupName);

            _cardsRepositoryMock.Setup(x => x.GetById(cardId))
                .ReturnsAsync(card);

            _groupsRepositoryMock.Setup(x => x.GetById(groupId))
                .ReturnsAsync(group);

            // act
            var (result, errors) = await _service.AddCard(cardId, groupId);

            // assert
            Assert.True(result);
            Assert.Empty(errors);
            _groupsRepositoryMock.Verify(x => x.AddCard(cardId, groupId), Times.Once);
        }

        [Theory]
        [InlineData(default, default)]
        [InlineData(-1, -1)]
        [InlineData(-111, -111)]
        [InlineData(default, -1)]
        [InlineData(default, 111)]
        [InlineData(-1, default)]
        [InlineData(111, default)]
        public async Task AddCard_CardIdGroupIdIsNotValid_ShouldReturnFalse(int cardId, int groupId)
        {
            // arrange

            // act
            var (result, errors) = await _service.AddCard(cardId, groupId);

            // assert
            Assert.False(result);
            Assert.NotEmpty(errors);
            _groupsRepositoryMock.Verify(x => x.AddCard(cardId, groupId), Times.Never);
        }
    }
}
