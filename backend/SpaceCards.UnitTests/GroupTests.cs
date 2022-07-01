using AutoFixture;
using SpaceCards.Domain;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests
{
    public class GroupTests
    {
        private readonly Fixture _fixture;
        private readonly StringFixture _stringFixture;

        public GroupTests()
        {
            _fixture = new Fixture();
            _stringFixture = new StringFixture();
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
        [MemberData(
            nameof(GroupDataGenerator.GenerateSetInvalidName),
            parameters: 20,
            MemberType = typeof(GroupDataGenerator))]
        public async Task Create_NameIsNotValid_ShouldReturnNullAndError(string name)
        {
            // arrange
            // act
            var (group, errors) = Group.Create(name);

            // assert
            Assert.Null(group);
            Assert.NotEmpty(errors);
        }
    }
}
