using AutoFixture;
using SpaceCards.Domain;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests
{
    public class GroupTests
    {
        private readonly Fixture _fixture;

        public GroupTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Create_IsValid_ShouldReturnGroup()
        {
            // arrange
            var name = _fixture.Create<string>();

            // act
            var (group, errors) = Group.Create(name);

            // assert
            Assert.NotNull(group);
            Assert.Empty(errors);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("      ")]
        [InlineData(null)]
        public async Task Create_NameIsNotValidNullOrWhitespace_ShouldReturnNullAndError(string name)
        {
            // arrange
            var excpectedError = $"'{nameof(name)}' cannot be null or whitespace.";

            // act
            var (group, errors) = Group.Create(name);

            // assert
            var error = errors.FirstOrDefault();
            Assert.Null(group);
            Assert.NotEmpty(errors);
            Assert.Equal(excpectedError, error);
        }

        [Theory]
        [InlineData(210)]
        public async Task Create_NameIsNotValidMoreThanMaxLength_ShouldReturnNullAndError(int nameLength)
        {
            // arrange
            var name = new string('a', nameLength);
            var excpectedError = $"'{nameof(name)}' more than {Group.MAX_NAME_LENGTH} characters.";

            // act
            var (group, errors) = Group.Create(name);

            // assert
            var error = errors.FirstOrDefault();
            Assert.Null(group);
            Assert.NotEmpty(errors);
            Assert.Equal(excpectedError, error);
        }
    }
}
