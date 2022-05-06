using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using SpaceCards.API.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SpaceCards.IntegrationTests
{
    public abstract class BaseControllerTests
    {
        protected readonly HttpClient _client;
        protected readonly Fixture _fixture;

        public BaseControllerTests()
        {
            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((_, configurationBuilder) =>
                    {
                        configurationBuilder.AddUserSecrets(typeof(BaseControllerTests).Assembly);
                    });
                });
            _client = app.CreateClient();
            _fixture = new Fixture();
        }
    }
}
