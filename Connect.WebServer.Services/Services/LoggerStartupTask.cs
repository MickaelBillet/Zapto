using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Connect.WebServer.Services
{
    public class LoggerStartupTask : IStartupTask
    {
        #region Services
        private IConfiguration Configuration { get; }
        #endregion

        #region Constructor
        public LoggerStartupTask(IConfiguration configuration)
        {
            this.Configuration = configuration;  
        }
        #endregion

        #region Method
        public async Task Execute()
        {
            await Task.Run(() =>
            {
                Log.Logger = new LoggerConfiguration()
                                    .ReadFrom.Configuration(this.Configuration)
                                    .Enrich.FromLogContext()
                                    .WriteTo.DatabaseSink(this.Configuration)
                                    .Filter.ByExcluding("RequestPath like '%/health%'")
                                    .CreateLogger();
            });
        }
        #endregion
    }
}
