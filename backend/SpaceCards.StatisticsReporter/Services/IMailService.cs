using SpaceCards.StatisticsReporter.Models;

namespace SpaceCards.StatisticsReporter.Services
{
    public interface IMailService
    {
        Task SendEmail(MailRequest mailRequest);
    }
}
