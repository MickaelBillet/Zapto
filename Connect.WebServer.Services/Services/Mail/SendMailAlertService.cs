using Framework.Infrastructure.Services;
using MailKit;
using Serilog;
using IMailService = Framework.Infrastructure.Services.IMailService;

namespace Connect.WebServer.Services
{
    public class SendMailAlertService : ISendMailAlertService
    {
        #region Services
        private IMailService MailService { get; }
        #endregion

        #region Constructor
        public SendMailAlertService(IMailService mailService)
        {
            this.MailService = mailService;
        }
        #endregion

        #region Methods
        public async Task SendAlertAsync(string locationId, string title, string body)
        {
            MailRequest mailRequest = new MailRequest()
            {
                Subject = $"{locationId} - {title}",
                Body = $"{body}",
                ToEmail = "mickael.billet@gmail.com",
            };

            bool sent = await this.MailService.SendEmailAsync(mailRequest, MailSent);
        }

        private void MailSent(object? sender, MessageSentEventArgs e)
        {
            Log.Information("Mail Sent");
        }
        #endregion
    }
}
