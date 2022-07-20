using Telegram.Bot;
using Telegram.Bot.Types;

namespace SpaceCards.StatisticsReporter
{
    public class TelegramStatisticsWorker : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<EmailStatisticsWorker> _logger;

        public TelegramStatisticsWorker(
            IServiceProvider provider,
            ILogger<EmailStatisticsWorker> logger)
        {
            _provider = provider;
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
                    currentDate.Seconds) == new TimeSpan(11, 48, 0))
                {
                    using var scope = _provider.CreateScope();
                    var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

                    Message message = await botClient.SendTextMessageAsync(
                        chatId: "@SpaceCardsAlerts",
                        text: "Trying *all the parameters* of `sendMessage` method",
                        cancellationToken: stoppingToken);

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
            }
        }
    }
}
