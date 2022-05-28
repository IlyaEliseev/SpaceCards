using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Respawn.Graph;
using SpaceCards.DataAccess.Postgre;
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
                await Task.Delay(500);
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
