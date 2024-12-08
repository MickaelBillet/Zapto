using Framework.Common.Services;
using Framework.Infrastructure.Services;
using MailKit;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using IMailService = Framework.Infrastructure.Services.IMailService;

namespace Connect.WebServer.Services
{
    public class SendMailAlertService : ISendMailAlertService
    {
        #region Services
        private IMailService MailService { get; }
        private ISecretService SecretService { get; }
        #endregion

        #region Constructor
        public SendMailAlertService(IServiceProvider serviceProvider)
        {
            this.MailService = serviceProvider.GetRequiredService<IMailService>();
            this.SecretService = serviceProvider.GetRequiredService<ISecretService>();
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
            string password = this.SecretService.GetSecret("MailPassword");
            string address = this.SecretService.GetSecret("MailAddress");
            await this.MailService.SendEmailAsync(mailRequest, MailSent, password, address);
        }

        private void MailSent(object? sender, MessageSentEventArgs e)
        {
            Log.Information("Mail Sent");
        }
        #endregion
    }
}
