namespace SpaceCards.StatisticsReporter
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);

                var currentDate = DateTimeOffset.UtcNow.TimeOfDay;

                if (new TimeSpan(
                    currentDate.Hours, 
                    currentDate.Minutes,
                    currentDate.Seconds) == new TimeSpan(17, 33, 0))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

            }
        }
    }
}