using SpaceCards.Domain.Interfaces;

namespace SpaceCards.API.Background_services
{
    public class GoogleAccesTokenRefresher : BackgroundService
    {
        private readonly ILogger<GoogleAccesTokenRefresher> _logger;
        private readonly IOAuthUsersRepository _externalAuthRepository;

        public GoogleAccesTokenRefresher(
            ILogger<GoogleAccesTokenRefresher> logger,
            IOAuthUsersRepository externalAuthRepository)
        {
            _logger = logger;
            _externalAuthRepository = externalAuthRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Work");
            }

            return Task.CompletedTask;
        }
    }
}
