using SpaceCards.BusinessLogic;
using SpaceCards.DataAccess.Postgre.Entites;
using SpaceCards.Domain.Model;
using SpaceCards.UnitTests.MemberData;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.UnitTests.Tests
{
    public class UserTests
    {
        public UserTests()
        {
            SecurityService = new SecurityService();
        }

        private SecurityService SecurityService { get; set; }

        [Fact]
        public async Task Create_ShouldReturnNewUser()
        {
            // arrange
            var userEmail = "testEmail@gmail.com";
            var userPassword = "W!*e4eee5e";
            var passwordHash = SecurityService.HashPassword(new UserEntity { }, userPassword);
            var userNickname = "User";

            // act
            var result = User.Create(userEmail, passwordHash, userNickname);

            // assert
            Assert.NotNull(result.Value);
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(userEmail, result.Value.Email);
            Assert.Equal(passwordHash, result.Value.PasswordHash);
            Assert.Equal(userNickname, result.Value.Nickname);
        }

        [Theory]
        [MemberData(
            nameof(UserDataGenerator.GenerateSetInvalidEmailPasswordHashDatetimeNickname),
            parameters: 10,
            MemberType = typeof(UserDataGenerator))]
        public async Task Create_EmailPasswordNicknameIsNotValid_ShouldReturnError(
            string email,
            string password,
            string nickname)
        {
            // arrange
            // act
            var result = User.Create(email, password, nickname);

            // assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
        }
    }
}
