using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Databases;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.IntegrationTests
{
    public class PostgressTestContainerFixture : IAsyncLifetime
    {
        public PostgreSqlTestcontainer Testcontainer { get; }

        public PostgressTestContainerFixture()
        {
            Testcontainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithName("SpaceCardsTestDb")
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "db",
                Username = "postgres",
                Password = "postgres",
            })
            .Build();
        }

        public Task InitializeAsync()
        {
            return Testcontainer.StartAsync();
        }

        public Task DisposeAsync()
        {
            return Testcontainer.DisposeAsync().AsTask();
        }
    }
}
