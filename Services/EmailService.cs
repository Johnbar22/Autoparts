using System.Net.Mail;
using System.Net;

namespace Shop_ex.Services
{
    public class EmailService : IEmailService
    {

        private readonly SmtpClient _smtpClient;
        private readonly string? _mailAppLogin;
        public EmailService()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            string? smtpMailServer = configuration.GetSection("MailConfig").GetSection("SmtpMailServer").Value;
            _mailAppLogin = configuration.GetSection("MailConfig").GetSection("MailAppLogin").Value;
            string? mailAppPassword = configuration.GetSection("MailConfig").GetSection("MailAppPassword").Value;
            bool enableSSL = configuration.GetSection("MailConfig").GetValue<bool>("EnableSSL");
            int smtpPort = configuration.GetSection("MailConfig").GetValue<int>("smtpPort");

            _smtpClient = new SmtpClient(smtpMailServer);
            _smtpClient.Credentials = new NetworkCredential(_mailAppLogin, mailAppPassword);
            _smtpClient.EnableSsl = enableSSL;
            _smtpClient.Port = smtpPort;
        }

        public void SendMail(List<string> targetEmails, string topic, string content)
        {
            foreach (var targetEmail in targetEmails)
            {
                try
                {
                    _smtpClient.Send(_mailAppLogin, targetEmail, topic, content);
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при отправке письма: " + ex.Message);

                }
            }
        }
    }
}
