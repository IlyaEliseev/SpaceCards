using AutoFixture;
using SpaceCards.API.Contracts;
using SpaceCards.Domain;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using Newtonsoft.Json;

namespace SpaceCards.IntegrationTests
{
    public class GroupsControllerTests : BaseControllerTests
    {
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

            var newGroup = _fixture.Build<CreateGroupRequest>()
                .Create();

            var postResponse = await _client.PostAsJsonAsync("groups", newGroup);

            var body = await postResponse.Content.ReadAsStreamAsync();
            using var sreamReader = new StreamReader(body);
            using var jsonReader = new JsonTextReader(sreamReader);

            var serializer = new JsonSerializer();

            var groupsResponse = serializer.Deserialize<CreateGroupRequest>(jsonReader);

            var updateGroup = _fixture.Create<UpdateGroupRequest>();

            // act
            var updateResponse = await _client.PutAsJsonAsync($"groups/{groupId}", updateGroup);

            // assert
            updateResponse.EnsureSuccessStatusCode();
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
