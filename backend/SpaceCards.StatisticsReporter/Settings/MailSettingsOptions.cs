namespace SpaceCards.StatisticsReporter.Settings
{
    public class MailSettingsOptions
    {
        public const string MailSettings = "MailSettings";

        public string Mail { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }
    }
}
