using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services.Services.ScheduleService
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
            Configuration = configuration;
        }

        #endregion

        #region Methods

        public override async Task ProcessInScope(IServiceScope scope)
        {
            Log.Information("DailyService");

            try
            {
                ISupervisorPlug supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorPlug>();
                IEnumerable<Plug> plugs = await supervisor.GetPlugs();

                //Reset the WorkingDuration daily
                foreach (Plug plug in plugs)
                {
                    ResultCode resultCode = await supervisor.ResetWorkingDuration(plug);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }
        #endregion
    }
}
