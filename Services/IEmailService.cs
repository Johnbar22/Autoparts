using System.Net.Mail;

namespace Shop_ex.Services
{
    public interface IEmailService
    {
        public void SendMail(string targetEmail, string topic, string content){}
    }
}