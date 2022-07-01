using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;
using Respawn.Graph;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.IntegrationTests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public DatabaseFixture()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("IntegrationTestsSettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(typeof(DatabaseFixture).Assembly)
                .Build();

            ConnectionString = builder.GetConnectionString("SpaceCardsDb");
        }

        public string ConnectionString { get; }

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
