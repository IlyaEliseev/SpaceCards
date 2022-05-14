using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaceCards.DataAccess.Postgre;
using SpaceCards.Domain;
using System.Net.Http;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests
{
    public abstract class BaseControllerTests
    {
        protected readonly HttpClient _client;
        protected readonly Fixture _fixture;
        protected readonly SpaceCardsDbContext _dbContext;
        protected readonly ICardsRepository _cardRepository;
        protected readonly IGroupsRepository _groupRepository;

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

            _client = app.CreateDefaultClient(new LoggingHandler(outputHelper));

            _fixture = new Fixture();

            _dbContext = app.Services.CreateScope().ServiceProvider.GetService<SpaceCardsDbContext>();
            var mapper = app.Services.CreateScope().ServiceProvider.GetService<IMapper>();

            _cardRepository = new CardsRepository(_dbContext, mapper);

            _groupRepository = new GroupsRepository(_dbContext, mapper);
        }
    }
}
