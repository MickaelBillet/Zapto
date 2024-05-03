using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class SendMailLogFileService : ISendMailWithFileService
    {
        #region Services
        private IMailService MailService { get; }
        private IKeyVaultService KeyVaultService { get; }
        #endregion

        #region Constructor
        public SendMailLogFileService(IServiceProvider serviceProvider)
        {
            this.MailService = serviceProvider.GetRequiredService<IMailService>();
            this.KeyVaultService = serviceProvider.GetRequiredService<IKeyVaultService>();
        }
        #endregion

        #region Methods
        public async Task Send(string directoryPath)
        {
            //Send a mail with a log file
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            IEnumerable<FileInfo> fileInfos = directoryInfo.GetFiles().OrderByDescending(file => file.LastWriteTime);
            if (fileInfos?.Count() > 0)
            {
                foreach(FileInfo file in fileInfos) 
                { 
                    MailRequest mailRequest = new MailRequest()
                    {
                        Attachments = new List<MailAttachment> { new MailAttachment(file.FullName) },
                        Body = string.Empty,
                        Subject = "Logs Connect " + Clock.Now,
                        ToEmail = "mickael.billet@gmail.com",
                    };
                    string password = this.KeyVaultService.GetSecret("MailPassword");
                    await this.MailService.SendEmailAsync(mailRequest, MailSent, password);
                }
            }
        }

        private void MailSent(object? sender, MailKit.MessageSentEventArgs e)
        {
            Log.Information("Mail Sent");
        }
        #endregion
    }
}
