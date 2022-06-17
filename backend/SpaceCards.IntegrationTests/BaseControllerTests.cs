using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Respawn.Graph;
using SpaceCards.DataAccess.Postgre;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests
{
    public abstract class BaseControllerTests : IAsyncLifetime
    {
        public BaseControllerTests(ITestOutputHelper outputHelper)
        {
            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((_, configurationBuilder) =>
                    {
                        configurationBuilder.AddUserSecrets(typeof(BaseControllerTests).Assembly);
                    });
                });

            Client = app.CreateDefaultClient(new LoggingHandler(outputHelper));

            Fixture = new Fixture();
            DbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<SpaceCardsDbContext>();
        }

        protected HttpClient Client { get; }

        protected Fixture Fixture { get; }

        protected SpaceCardsDbContext DbContext { get; }

        protected async Task<int> MakeCard()
        {
            var card = Fixture.Build<DataAccess.Postgre.Entites.Card>()
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
            var group = Fixture.Build<DataAccess.Postgre.Entites.Group>()
                .Without(x => x.Id)
                .Without(x => x.Cards)
                .Create();

            DbContext.Groups.Add(group);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            return group.Id;
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

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            var connectionString = DbContext.Database.GetConnectionString();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();

                await checkpoint.Reset(conn);
                await Task.Delay(1000);
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
