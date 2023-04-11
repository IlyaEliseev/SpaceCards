using SpaceCards.BusinessLogic;
using SpaceCards.DataAccess.Postgre.Repositories;
using SpaceCards.Domain.Interfaces;

namespace SpaceCards.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICardsRepository, CardsRepository>();
            services.AddScoped<IGroupsRepository, GroupsRepository>();
            services.AddScoped<IGuessedCardRepository, GuessedCardRepository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IUsersAccountRepository, UsersAccountRepository>();
            services.AddScoped<ISessionsRepository, SessionsRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<ICardsService, CardsService>();
            service.AddScoped<IGroupsService, GroupsService>();
            service.AddScoped<IGuessedCardsService, GuessedCardsService>();
            service.AddScoped<IStatisticsService, StatisticsService>();
            service.AddScoped<IUsersAccountService, UsersAccountService>();
            service.AddScoped<SecurityService>();

            return service;
        }
    }
}
