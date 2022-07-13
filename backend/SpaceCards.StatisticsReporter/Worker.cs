using SpaceCards.StatisticsReporter.Models;
using SpaceCards.StatisticsReporter.Services;
using StackExchange.Redis;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SpaceCards.StatisticsReporter
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider provider, ILogger<Worker> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6380");
            IDatabase db = redis.GetDatabase();

            string value = "abcdefg";
            db.StringSet("mykey", value);
            string value1 = db.StringGet("mykey");
            Console.WriteLine(value1);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);

                var currentDate = DateTimeOffset.UtcNow.TimeOfDay;

                if (new TimeSpan(
                    currentDate.Hours,
                    currentDate.Minutes,
                    currentDate.Seconds) == new TimeSpan(18, 21, 0))
                {
                    using var scope = _provider.CreateScope();
                    var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                    var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

                    Message message = await botClient.SendTextMessageAsync(
                        chatId: "@SpaceCardsAlerts",
                        text: "Trying *all the parameters* of `sendMessage` method",
                        cancellationToken: stoppingToken);

                    var mailRequest = new MailRequest
                    {
                        ToEmail = "scorpion_89freez@mail.ru",
                        Body = "Test message",
                        Subject = "Hello"
                    };

                    await mailService.SendEmail(mailRequest);

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
            }
        } 
    }
}