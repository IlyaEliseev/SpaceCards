using SpaceCards.StatisticsReporter;
using SpaceCards.StatisticsReporter.Settings;
using SpaceCards.StatisticsReporter.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext ,services) =>
    {
        services.AddSingleton<IMailService, MailService>();

        services.AddHostedService<Worker>();

        services.Configure<MailSettingsOptions>(
            hostContext.Configuration
            .GetSection(MailSettingsOptions.MailSettings));

    })
    .Build();

await host.RunAsync();
