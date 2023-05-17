﻿using AutoFixture;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using SpaceCards.API;
using SpaceCards.DataAccess.Postgre;
using SpaceCards.DataAccess.Postgre.Entites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Respawn.Graph;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace SpaceCards.IntegrationTests.Tests
{
    public abstract class BaseControllerTests : ControllerBase, IAsyncLifetime
    {
        private static readonly string _baseDirecotry = AppContext.BaseDirectory;
        private static readonly string _path = Directory.GetParent(_baseDirecotry).Parent.Parent.Parent.FullName;

        public BaseControllerTests(ITestOutputHelper outputHelper)
        {
            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, configurationBuilder) =>
                    {
                        var configutarion = configurationBuilder
                            .SetBasePath(_path)
                            .AddJsonFile("appsettings.Test.json")
                            .AddEnvironmentVariables()
                            .Build();

                        UserId = configutarion
                            .GetSection("TestUserId")
                            .Value;

                        JwtTokenSecret = configutarion
                            .GetSection("JwtTokenSecret")
                            .Value;

                        var test = configutarion.GetSection("Name").Value;

                        ConnectionString = configutarion.GetConnectionString("SpaceCardsDb");
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

        protected string ConnectionString { get; set; }

        protected async Task SignIn()
        {
            var userId = await GetUserId();

            var userIdInformation = new UserInformation(UserNickname, userId);
            var token = CreateAccessToken(userIdInformation);
            var uri = new Uri("https://localhost:49394/");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(uri, new Cookie("_sp_i", token));
            Client.DefaultRequestHeaders.Add("cookie", cookieContainer.GetCookieHeader(uri));
            //Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            //    "Bearer",
            //    token);
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

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
            }
        }

        private static Checkpoint checkpoint = new Checkpoint
        {
            TablesToIgnore = new[]
            {
                new Table("__EFMigrationsHistory")
            },

            DbAdapter = DbAdapter.Postgres
        };
    }
}
