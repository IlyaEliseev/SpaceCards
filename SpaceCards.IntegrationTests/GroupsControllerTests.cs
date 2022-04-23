using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SpaceCards.API.Contracts;
using SpaceCards.Domain;
using System;
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
        private readonly Mock<IGroupsService> _groupsServiceMock;

        public GroupsControllerTests()
        {
            _groupsServiceMock = new Mock<IGroupsService>();

            var app = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var cardService = services.SingleOrDefault(s => s.ServiceType == typeof(IGroupsService));
                    services.Remove(cardService);
                    services.AddScoped(_ => _groupsServiceMock.Object);
                });
            });

            _client = app.CreateClient();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_ShouldReturnOk()
        {
            // arrange
            _groupsServiceMock.Setup(x => x.Get()).ReturnsAsync(Array.Empty<Group>());

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

            _groupsServiceMock.Setup(c => c.Create(group.Name))
                .ReturnsAsync((1, Array.Empty<string>()));

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

            _groupsServiceMock.Setup(x => x.Create(group.Name))
                .ReturnsAsync((default, errors));

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

            _groupsServiceMock.Setup(x => x.Update(groupId, group.Name))
                .ReturnsAsync((true, Array.Empty<string>()));

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

            _groupsServiceMock.Setup(x => x.Update(groupId, group.Name))
                .ReturnsAsync((false, errors));

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

            _groupsServiceMock.Setup(x => x.Delete(groupId))
                .ReturnsAsync((true, Array.Empty<string>()));

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
            _groupsServiceMock.Setup(x => x.Delete(groupId)).ReturnsAsync((false, errors));

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

            _groupsServiceMock.Setup(x => x.AddCard(cardId, groupId))
                .ReturnsAsync((true, Array.Empty<string>()));

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

            _groupsServiceMock.Setup(x => x.AddCard(cardId, groupId))
                .ReturnsAsync((false, errors));

            // act
            var response = await _client.PostAsJsonAsync($"groups/{groupId}/cards?cardId={cardId}", cardId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
