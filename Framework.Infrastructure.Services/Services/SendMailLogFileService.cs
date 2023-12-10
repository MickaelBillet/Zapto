using Framework.Core.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class SendMailLogFileService : ISendMailLogFileService
    {
        #region Services
        private IMailService MailService { get; }
        #endregion

        #region Constructor
        public SendMailLogFileService(IMailService mailService)
        {
            this.MailService = mailService;
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
                using (FileStream fs = new FileStream(fileInfos.ToList()[0].FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] result = new byte[fs.Length];
                    await fs.ReadAsync(result, 0, (int)fs.Length);

                    FormFile file = new FormFile(fs, 0, fs.Length, fs.Name, Path.GetFileName(fs.Name))
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "application/octet-stream"
                    };

                    MailRequest mailRequest = new MailRequest()
                    {
                        Attachments = new List<IFormFile>() { file },
                        Body = string.Empty,
                        Subject = "Logs Connect " + Clock.Now,
                        ToEmail = "mickael.billet@gmail.com",
                    };

                    await this.MailService.SendEmailAsync(mailRequest, MailSent);
                }
            }
        }

        private void MailSent(object sender, MailKit.MessageSentEventArgs e)
        {
        }
        #endregion
    }
}
