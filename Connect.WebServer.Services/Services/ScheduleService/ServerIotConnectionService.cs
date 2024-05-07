using Connect.Application;
using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Framework.Core.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class ServerIotConnectionService : ScheduledService
    {
        #region Properties
        private HostedServiceHealthCheck? HostedServiceHealthCheck { get; }
        #endregion

        #region Constructor

        public ServerIotConnectionService(IServiceScopeFactory serviceScopeFactory, HostedServiceHealthCheck hostedServiceHealthCheck) : base(serviceScopeFactory, 10)
        {
            HostedServiceHealthCheck = hostedServiceHealthCheck;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Scoped service which is launched at the startup of the app
        /// Receive the status of the Arduino Server (Iot Server)
        /// </summary>
        /// <returns></returns>
        public override async Task ProcessInScope(IServiceScope scope)
        {
            Log.Information("ServerIotConnectionService.ProcessInScope");

            try
            {
                IApplicationServerIotServices applicationServerIotServices = scope.ServiceProvider.GetRequiredService<IApplicationServerIotServices>();
                ISupervisorServerIotStatus supervisorServerIotStatus = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryServerIotStatus>().CreateSupervisor();
                SystemStatus? status = await applicationServerIotServices.ReceiveStatusAsync();
                if (status != null)
                {
                    if (HostedServiceHealthCheck != null)
                    {
                        status.Date = Clock.Now;
                        HostedServiceHealthCheck.SetStatus(status);

                        if (status.Status == Domain.SystemStatus.STARTED)
                        {
                            await supervisorServerIotStatus.AddServerIotStatus(new ServerIotStatus()
                            {
                                Id = Guid.NewGuid().ToString(),
                                IpAddress = status.IpAddress,
                                ConnectionDate = status.Date,
                                Date = Clock.Now,
                            });
                        }
                    }
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
