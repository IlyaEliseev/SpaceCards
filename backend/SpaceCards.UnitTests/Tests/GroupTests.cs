using AutoFixture;
using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.MemberData;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests.Tests
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
            var userId = _fixture.Create<Guid>();

            // act
            var result = Group.Create(name, userId);

            // assert
            Assert.NotNull(result.Value);
            Assert.Equal(name, result.Value.Name);
            Assert.Equal(userId, result.Value.UserId);
            Assert.False(result.IsFailure);
        }

        [Theory]
        [MemberData(
            nameof(GroupDataGenerator.GenerateSetInvalidName),
            parameters: 20,
            MemberType = typeof(GroupDataGenerator))]
        public async Task Create_NameIsNotValid_ShouldReturnNullAndError(string name)
        {
            // arrange
            var userId = _fixture.Create<Guid>();

            // act
            var result = Group.Create(name, userId);

            // assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error);
        }
    }
}
