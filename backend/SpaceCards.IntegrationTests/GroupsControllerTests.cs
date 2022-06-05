﻿using AutoFixture;
using SpaceCards.API.Contracts;
using SpaceCards.Domain;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests
{
    public class GroupsControllerTests : BaseControllerTests
    {
        public GroupsControllerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task Get_ShouldReturnOk()
        {
            // arrange
            // act
            var response = await Client.GetAsync("groups");

            // arrange
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetById_ShouldReturnOk()
        {
            // arrange
            var groupId = await MakeGroup();

            // act
            var response = await Client.GetAsync($"groups/{groupId}");

            // arrange
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(default)]
        [InlineData(-1)]
        [InlineData(-111)]
        public async Task GetById_ShouldReturnBadRequest(int groupId)
        {
            // arrange
            // act
            var response = await Client.GetAsync($"groups/{groupId}");

            // arrange
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            // arrange
            var group = Fixture.Create<CreateGroupRequest>();

            // act
            var response = await Client.PostAsJsonAsync("groups", group);

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
            var group = Fixture.Build<CreateGroupRequest>()
                .With(x => x.Name, name)
                .Create();

            var errors = Fixture.CreateMany<string>().ToArray();

            // act
            var response = await Client.PostAsJsonAsync("groups", group);

            // arrange
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            // arrange
            var groupId = await MakeGroup();

            var updatedGroup = Fixture.Create<UpdateGroupRequest>();

            // act
            var updateResponse = await Client.PutAsJsonAsync($"groups/{groupId}", updatedGroup);

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
            var group = Fixture.Build<UpdateGroupRequest>()
                .With(x => x.Name, name)
                .Create();

            var errors = Fixture.CreateMany<string>().ToArray();

            // act
            var response = await Client.PutAsJsonAsync($"groups/{groupId}", group);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            // arrange
            var groupId = await MakeGroup();

            // act
            var response = await Client.DeleteAsync($"groups/{groupId}");

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
            // act
            var response = await Client.DeleteAsync($"groups/{groupId}");

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddCard_ShouldReturnOk()
        {
            // arrange
            var cardId = await MakeCard();
            var groupId = await MakeGroup();

            // act
            var response = await Client.PostAsJsonAsync($"groups/{groupId}/cards?cardId={cardId}", cardId);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(default, default)]
        [InlineData(-1, -1)]
        [InlineData(-111, -111)]
        [InlineData(default, -1)]
        [InlineData(default, -111)]
        [InlineData(-1, default)]
        [InlineData(-111, default)]
        public async Task AddCard_ShouldReturnBadRequest(int cardId, int groupId)
        {
            // arrange
            // act
            var response = await Client.PostAsJsonAsync($"groups/{groupId}/cards?cardId={cardId}", cardId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetRandomCards_ShouldReturnOk()
        {
            // arrange
            var countCards = 10;

            // act
            var response = await Client.GetAsync($"groups/cards?countCards={countCards}");

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-111)]
        [InlineData(0)]
        public async Task GetRandomCards_ShouldReturnBadRequest(int countCards)
        {
            // arrange
            // act
            var response = await Client.GetAsync($"groups/cards?countCards={countCards}");

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}