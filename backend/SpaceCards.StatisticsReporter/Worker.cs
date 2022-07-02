using SpaceCards.StatisticsReporter.Models;
using SpaceCards.StatisticsReporter.Services;
using StackExchange.Redis;

namespace SpaceCards.StatisticsReporter
{
    public class Worker : BackgroundService
    {
        private readonly IMailService _mailService;
        private readonly ILogger<Worker> _logger;

        public Worker(IMailService mailService,ILogger<Worker> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6380,sslprotocols=tls12,,abortConnect=False");
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
                    var mailRequest = new MailRequest
                    {
                        ToEmail = "scorpion_89freez@mail.ru",
                        Body = "Test message",
                        Subject = "Hello"
                    };

                    await _mailService.SendEmail(mailRequest);

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
            }
        } 
    }
}