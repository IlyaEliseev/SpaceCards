using SpaceCards.StatisticsReporter;
using SpaceCards.StatisticsReporter.Settings;
using SpaceCards.StatisticsReporter.Services;
using Telegram.Bot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext ,services) =>
    {
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<ITelegramBotClient>(x =>
        {
            return new TelegramBotClient(hostContext.Configuration.GetSection("TelegramToken").Value);
        });

        services.AddHostedService<EmailStatisticsWorker>();
        services.AddHostedService<TelegramStatisticsWorker>();

        services.Configure<MailSettingsOptions>(
            hostContext.Configuration
            .GetSection(MailSettingsOptions.MailSettings));

    })
    .Build();

await host.RunAsync();
