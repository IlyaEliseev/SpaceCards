using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SpaceCards.StatisticsReporter.Models;
using SpaceCards.StatisticsReporter.Settings;

namespace SpaceCards.StatisticsReporter.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettingsOptions _mailSettings;

        public MailService(IOptions<MailSettingsOptions> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmail(MailRequest mailRequest)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = mailRequest.Subject,
            };

            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(
                _mailSettings.Host,
                _mailSettings.Port,
                SecureSocketOptions.StartTls);

            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
