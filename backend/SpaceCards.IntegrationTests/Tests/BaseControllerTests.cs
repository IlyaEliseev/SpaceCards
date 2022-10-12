using AutoFixture;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaceCards.API;
using SpaceCards.DataAccess.Postgre;
using SpaceCards.DataAccess.Postgre.Entites;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests.Tests
{
    public abstract class BaseControllerTests : IClassFixture<DatabaseFixture>
    {
        public BaseControllerTests(ITestOutputHelper outputHelper)
        {
            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, configurationBuilder) =>
                    {
                        var configutarion = configurationBuilder
                        .AddUserSecrets(typeof(BaseControllerTests).Assembly)
                        .Build();

                        UserId = configutarion
                            .GetSection("TestUserId")
                            .Value;

                        JwtTokenSecret = configutarion
                            .GetSection("JwtTokenSecret")
                            .Value;
                    });
                });

            Client = app.CreateDefaultClient(new LoggingHandler(outputHelper));
            Fixture = new Fixture();
            DbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<SpaceCardsDbContext>();
        }

        protected HttpClient Client { get; }

        protected Fixture Fixture { get; }

        protected string UserId { get; set; }

        protected string UserEmail => "testEmail@gmail.com";

        protected string UserNickname => "Nickname";

        protected string UserPassword => "Yq3qq!4qq&";

        protected string JwtTokenSecret { get; set; }

        protected SpaceCardsDbContext DbContext { get; }

        protected async Task SignIn()
        {
            var userId = await GetUserId();

            var userIdInformation = new UserInformation(UserNickname, userId);
            var token = CreateAccessToken(userIdInformation);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                BaseSchema.NAME,
                token);
        }

        protected async Task<Guid> GetUserId()
        {
            Guid.TryParse(UserId, out var userId);
            return userId;
        }

        protected async Task<int> MakeCard()
        {
            var userId = await GetUserId();

            var card = Fixture.Build<DataAccess.Postgre.Entites.CardEntity>()
                            .With(x => x.UserId, userId)
                            .Without(x => x.Id)
                            .Without(x => x.Group)
                            .Without(x => x.GroupId)
                            .Create();

            DbContext.Cards.Add(card);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            return card.Id;
        }

        protected async Task<int> MakeGroup()
        {
            var userId = await GetUserId();

            var group = Fixture.Build<DataAccess.Postgre.Entites.GroupEntity>()
                .With(x => x.UserId, userId)
                .Without(x => x.Id)
                .Without(x => x.Cards)
                .Create();

            DbContext.Groups.Add(group);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            return group.Id;
        }

        protected async Task<int> MakeCardGuessingStatistics()
        {
            var userId = await GetUserId();
            var rnd = new Random();

            var cardGuessingStatistics = Fixture
                .Build<DataAccess.Postgre.Entites.CardGuessingStatisticsEntity>()
                .Without(x => x.Id)
                .With(x => x.UserId, userId)
                .With(x => x.Success, rnd.Next(0, 2))
                .Create();

            await DbContext.CardsGuessingStatistics.AddAsync(cardGuessingStatistics);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            return cardGuessingStatistics.Id;
        }

        protected async Task<(int GroupId, int CardId)> AddCardInGroup()
        {
            var cardId = await MakeCard();
            var groupId = await MakeGroup();

            var card = await DbContext.Cards.FirstOrDefaultAsync(x => x.Id == cardId);
            card.GroupId = groupId;
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            return (groupId, cardId);
        }

        protected async Task<List<int>> GenerateGroups(int groupsCount)
        {
            var groupsId = new List<int>();

            for (int i = 0; i < groupsCount; i++)
            {
                var groupId = await MakeGroup();
                groupsId.Add(groupId);
            }

            return groupsId;
        }

        protected async Task GenerateCardsInGroups(int groupsCount, int cardsCount)
        {
            var groupsId = await GenerateGroups(groupsCount);

            for (int i = 0; i < cardsCount; i++)
            {
                await MakeCard();
            }

            var cards = await DbContext.Cards
                .ToArrayAsync();

            foreach (var groupId in groupsId)
            {
                foreach (var card in cards)
                {
                    card.GroupId = groupId;
                    await DbContext.SaveChangesAsync();
                }
            }
        }

        protected async Task<(string AccessToken, string RefreshToken)> MakeSession()
        {
            var userId = await GetUserId();

            var userInformation = new UserInformation(UserNickname, userId);

            var accessToken = CreateAccessToken(userInformation);
            var refreshToken = CreateRefreshToken(userInformation);

            var session = new SessionEntity
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = userId
            };

            await DbContext.Sessions.AddAsync(session);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            return (accessToken, refreshToken);
        }

        protected string CreateAccessToken(UserInformation information)
        {
            var accsessToken = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm())
                      .WithSecret(JwtTokenSecret)
                      .ExpirationTime(DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim(ClaimTypes.Name, information.Nickname)
                      .AddClaim(ClaimTypes.NameIdentifier, information.UserId)
                      .WithVerifySignature(true)
                      .Encode();

            return accsessToken;
        }

        protected string CreateRefreshToken(UserInformation information)
        {
            var refreshToken = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(JwtTokenSecret)
                    .ExpirationTime(DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeSeconds())
                    .AddClaim(ClaimTypes.Name, information.Nickname)
                    .AddClaim(ClaimTypes.NameIdentifier, information.UserId)
                    .WithVerifySignature(true)
                    .Encode();

            return refreshToken;
        }
    }
}
