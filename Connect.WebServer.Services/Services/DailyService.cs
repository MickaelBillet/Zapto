using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using IMailService = Framework.Infrastructure.Services.IMailService;

namespace Connect.WebServer.Services
{
    public class DailyService : CronScheduledService
    {
        #region Properties 

        protected override string Schedule => "59 23 * * *";

        //protected override string Schedule => "*/1 * * * *";

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public DailyService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Methods

        public override async Task ProcessInScope(IServiceScope scope)
        {
            Log.Information("DailyService");

            try
            {
                ISupervisorPlug supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorPlug>();
                IMailService mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                IEnumerable<Plug> plugs = await supervisor.GetPlugs();

                //Reset the WorkingDuration daily
                foreach(Plug plug in plugs)
                {
                    ResultCode resultCode = await supervisor.ResetWorkingDuration(plug);
                }

                //Send a mail with a log file
                //DirectoryInfo directoryInfo = new DirectoryInfo(this.Configuration["LogDirectoryPath"]);

                //IEnumerable<FileInfo> fileInfos = directoryInfo.GetFiles().OrderByDescending(file => file.LastWriteTime);

                //if (fileInfos?.Count() > 0)
                //{
                //    using (FileStream fs = new FileStream(fileInfos.ToList()[0].FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                //    {
                //        byte[] result = new byte[fs.Length];
                //        await fs.ReadAsync(result, 0, (int)fs.Length);

                //        FormFile file = new FormFile(fs, 0, fs.Length, fs.Name, Path.GetFileName(fs.Name))
                //        {
                //            Headers = new HeaderDictionary(),
                //            ContentType = "application/octet-stream"
                //        };

                //        MailRequest mailRequest = new MailRequest()
                //        {
                //            Attachments = new List<IFormFile>() { file },
                //            Body = string.Empty,
                //            Subject = "Logs Connect " + Clock.Now,
                //            ToEmail = "mickael.billet@gmail.com",
                //        };

                //        await mailService.SendEmailAsync(mailRequest, MailSent);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        private void MailSent(object? sender, MessageSentEventArgs arg)
        {
        }

        #endregion
    }
}
