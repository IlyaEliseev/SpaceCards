using AutoFixture;
using SpaceCards.API.Contracts;
using SpaceCards.IntegrationTests.MemberData;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests.Tests
{
    public class GroupsControllerTests : BaseControllerTests
    {
        public GroupsControllerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task Get_groups_is_response_Ok()
        {
            // arrange
            await SignIn();

            // act
            var response = await Client.GetAsync("groups");

            // arrange
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_group_by_valid_id_is_response_Ok()
        {
            // arrange
            await SignIn();
            var groupId = await MakeGroup();

            // act
            var response = await Client.GetAsync($"groups/{groupId}");

            // arrange
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(GroupsDataGenerator.GenerateSetInvalidGroupId),
            MemberType = typeof(GroupsDataGenerator))]
        public async Task Get_group_by_invalid_id_is_response_BadRequest(int groupId)
        {
            // arrange
            await SignIn();

            // act
            var response = await Client.GetAsync($"groups/{groupId}");

            // arrange
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_group_with_a_valid_parameters_is_response_Ok()
        {
            // arrange
            await SignIn();
            var group = Fixture.Create<CreateGroupRequest>();

            // act
            var response = await Client.PostAsJsonAsync("groups", group);

            // arrange
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(GroupsDataGenerator.GenerateSetInvalidName),
            MemberType = typeof(GroupsDataGenerator))]
        public async Task Create_group_with_a_invalid_parameters_is_response_BadRequest(string name)
        {
            // arrange
            await SignIn();

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
        public async Task Update_group_by_valid_id_with_valid_parameters_is_response_Ok()
        {
            // arrange
            await SignIn();
            var groupId = await MakeGroup();

            var updatedGroup = Fixture.Create<UpdateGroupRequest>();

            // act
            var updateResponse = await Client.PutAsJsonAsync($"groups/{groupId}", updatedGroup);

            // assert
            updateResponse.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(GroupsDataGenerator.GenerateSetInvalidGroupIdName),
            MemberType = typeof(GroupsDataGenerator))]
        public async Task Update_group_by_invalid_id_with_invalid_parameters_is_response_BadRequest(
            int groupId,
            string name)
        {
            // arrange
            await SignIn();
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
        public async Task Delete_group_by_valid_id_is_response_Ok()
        {
            // arrange
            await SignIn();
            var groupId = await MakeGroup();

            // act
            var response = await Client.DeleteAsync($"groups/{groupId}");

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(GroupsDataGenerator.GenerateSetInvalidGroupId),
            MemberType = typeof(GroupsDataGenerator))]
        public async Task Delete_group_by_invalid_id_is_response_BadRequest(int groupId)
        {
            // arrange
            await SignIn();

            // act
            var response = await Client.DeleteAsync($"groups/{groupId}");

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Add_card_in_group_with_valid_card_id_and_group_id_is_response_Ok()
        {
            // arrange
            await SignIn();
            var cardId = await MakeCard();
            var groupId = await MakeGroup();

            // act
            var response = await Client.PostAsJsonAsync($"groups/{groupId}/cards?cardId={cardId}", cardId);

            // assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(
            nameof(GroupsDataGenerator.GenerateSetInvalidCardIdGroupId),
            MemberType = typeof(GroupsDataGenerator))]
        public async Task Add_card_in_group_with_invalid_card_id_and_group_id_is_response_BadRequest(
            int cardId,
            int groupId)
        {
            // arrange
            await SignIn();

            // act
            var response = await Client.PostAsJsonAsync($"groups/{groupId}/cards?cardId={cardId}", cardId);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_random_cards_from_all_groups_with_valid_count_cards_is_response_Ok()
        {
            // arrange
            await SignIn();
            var countCards = 10;
            var countGroups = 2;
            await GenerateCardsInGroups(countGroups, countCards);

            // act
            var response1 = await Client.GetAsync($"groups/randomCards?countCards={countCards}");
            var response2 = await Client.GetAsync($"groups/randomCards?countCards={countCards}");

            var cardsResponse1 = await response1.Content
                .ReadFromJsonAsync<DataAccess.Postgre.Entites.CardEntity[]>();

            var cardsResponse2 = await response2.Content
                .ReadFromJsonAsync<DataAccess.Postgre.Entites.CardEntity[]>();

            var result = cardsResponse1.SequenceEqual(cardsResponse2);

            // assert
            response1.EnsureSuccessStatusCode();
            response2.EnsureSuccessStatusCode();
            Assert.Equal(countCards, cardsResponse1.Length);
            Assert.Equal(countCards, cardsResponse2.Length);
            Assert.True(!result);
        }

        [Theory]
        [MemberData(
            nameof(GroupsDataGenerator.GenerateSetInvalidCountCards),
            MemberType = typeof(GroupsDataGenerator))]
        public async Task Get_random_cards_from_all_groups_with_invalid_count_cards_is_response_BadRequest(
            int countCards)
        {
            // arrange
            await SignIn();

            // act
            var response = await Client.GetAsync($"groups/randomCards?countCards={countCards}");

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
