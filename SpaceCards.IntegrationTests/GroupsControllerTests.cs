using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using SpaceCards.API.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.IntegrationTests
{
    public class GroupsControllerTests
    {
        private readonly HttpClient _client;
        private readonly Fixture _fixture;

        public GroupsControllerTests()
        {
            var app = new WebApplicationFactory<Program>();
            _client = app.CreateClient();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_ShouldReturnOk()
        {
            // arrange

            // act
            var response = await _client.GetAsync("groups");

            // arrange
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            // arrange
            var group = _fixture.Create<CreateGroupRequest>();

            // act
            var response = await _client.PostAsJsonAsync("groups", group);

            // arrange
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("      ")]
        public async Task Create_ShouldReturnBadRequest(string name)
        {
            // arrange
            var group = _fixture.Build<CreateGroupRequest>()
                .With(x => x.Name, name)
                .Create();

            var errors = _fixture.CreateMany<string>().ToArray();

            // act
            var response = await _client.PostAsJsonAsync("groups", group);

            // arrange
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            // arrange
            var groupId = _fixture.Create<int>();
            var group = _fixture.Create<UpdateGroupRequest>();

            // act
            var response = await _client.PutAsJsonAsync($"groups/{groupId}", group);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(default, null)]
        [InlineData(default, "")]
        [InlineData(default, " ")]
        [InlineData(default, "   ")]
        public async Task Update_ShouldReturnBadRequest(int groupId, string name)
        {
            // arrange
            var group = _fixture.Build<UpdateGroupRequest>()
                .With(x => x.Name, name)
                .Create();

            var errors = _fixture.CreateMany<string>().ToArray();

            // act
            var response = await _client.PutAsJsonAsync($"groups/{groupId}", group);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            // arrange
            var groupId = _fixture.Create<int>();

            // act
            var response = await _client.DeleteAsync($"groups/{groupId}");

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(default)]
        [InlineData(-1)]
        [InlineData(-111)]
        public async Task Delete_ShouldReturnBadRequest(int groupId)
        {
            // arrange
            var errors = _fixture.CreateMany<string>().ToArray();

            // act
            var response = await _client.DeleteAsync($"groups/{groupId}");

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddCard_ShouldReturnOk()
        {
            // arrange
            var groupId = _fixture.Create<int>();
            var cardId = _fixture.Create<int>();

            // act
            var response = await _client.PostAsJsonAsync($"groups/{groupId}/cards?cardId={cardId}", cardId);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(default, default)]
        [InlineData(-1, -1)]
        [InlineData(-111, -111)]
        [InlineData(default, -1)]
        [InlineData(default, 111)]
        [InlineData(-1, default)]
        [InlineData(111, default)]
        public async Task AddCard_ShouldReturnBadRequest(int cardId, int groupId)
        {
            // arrange
            var errors = _fixture.CreateMany<string>().ToArray();

            // act
            var response = await _client.PostAsJsonAsync($"groups/{groupId}/cards?cardId={cardId}", cardId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
