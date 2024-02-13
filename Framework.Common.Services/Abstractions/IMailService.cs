using MailKit;
using System;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(MailRequest mailRequest, EventHandler<MessageSentEventArgs> smtp_sent);
    }
}
