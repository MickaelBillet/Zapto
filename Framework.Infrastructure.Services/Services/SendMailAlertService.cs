using MailKit;
using Serilog;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
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
            MailRequest mailRequest = new MailRequest();

            mailRequest.Subject = $"{locationId} - {title}";
            mailRequest.Body = $"{body}";

            bool sent = await this.MailService.SendEmailAsync(mailRequest, MailSent);
        }

        private void MailSent(object sender, MessageSentEventArgs e)
        {
            Log.Information("Mail Sent");
        }
        #endregion
    }
}
