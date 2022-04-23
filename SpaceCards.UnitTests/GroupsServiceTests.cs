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
        private readonly GroupsService _service;

        public GroupsServiceTests()
        {
            _fixture = new Fixture();
            _groupsRepositoryMock = new Mock<IGroupsRepository>();
            _service = new GroupsService(_groupsRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_GroupIsValid_ShouldCreateNewGroup()
        {
            // arrange
            var name = _fixture.Create<string>();
            var expectedGroupId = _fixture.Create<int>();

            var group = Group.Create(name);

            _groupsRepositoryMock.Setup(x => x.Add(group))
                .ReturnsAsync(expectedGroupId);

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
        public async Task Get_GetIsValid_ReturnGroups()
        {
            // arrange
            var name = _fixture.Create<string>();
            var group = Group.Create(name);

            var expectedGroups = Enumerable.Range(1, 5).Select(x => group).ToArray();

            _groupsRepositoryMock.Setup(x => x.Get()).ReturnsAsync(expectedGroups);

            // act
            var groups = await _service.Get();

            // assert
            Assert.NotNull(groups);
            Assert.Equal(expectedGroups, groups);
            _groupsRepositoryMock.Verify(x => x.Get(), Times.Once);
        }

        [Fact]
        public async Task Delete_DeleteIsValid_ReturnTrue()
        {
            // arrange
            var groupId = _fixture.Create<int>();
            var name = _fixture.Create<string>();
            var group = Group.Create(name);

            _groupsRepositoryMock.Setup(x => x.GetById(groupId))
                .ReturnsAsync((group, Array.Empty<string>()));

            // act
            var (result, errors) = await _service.Delete(groupId);

            // assert
            Assert.True(result);
            _groupsRepositoryMock.Verify(x => x.Delete(groupId), Times.Once);
        }

        [Theory]
        [InlineData(default)]
        [InlineData(-1)]
        [InlineData(-112)]
        public async Task Delete_GroupIdIsNotValid_ReturnFalseAndErrors(int groupId)
        {
            // arrange

            // act
            var (result, errors) = await _service.Delete(groupId);

            // assert
            Assert.False(result);
            _groupsRepositoryMock.Verify(x => x.Delete(groupId), Times.Never);
        }

        [Fact]
        public async Task Update_GroupIdNameIsValid_ShouldReturnTrue()
        {
            // arrange
            var name = _fixture.Create<string>();
            var groupId = _fixture.Create<int>();
            var group = Group.Create(name);

            var newName = "test";

            _groupsRepositoryMock.Setup(x => x.GetById(groupId))
                .ReturnsAsync((group, Array.Empty<string>()));

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
            var group = Group.Create(name);
            var contextErrors = _fixture.CreateMany<string>(1).ToArray();

            _groupsRepositoryMock.Setup(x => x.GetById(groupId))
                .ReturnsAsync((null, contextErrors));

            // act
            var (result, errors) = await _service.Update(groupId, name);
            var errorsCount = errors.Count();

            // assert
            Assert.False(result);
            Assert.Equal(4, errorsCount);
            _groupsRepositoryMock.Verify(x => x.Update(group), Times.Never);
        }

        [Fact]
        public async Task AddCard_CardIdGroupIdIsValid_ShouldReturnTrue()
        {
            // arrange
            var cardId = _fixture.Create<int>();
            var groupId = _fixture.Create<int>();

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
