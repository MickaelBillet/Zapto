using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Framework.Core.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal class ApplicationServerIotServices : IApplicationServerIotServices
    {
        #region Services
        private IWSMessageManager? WSMessageManager { get; }
        private IServiceScopeFactory? ServiceScopeFactory { get; }
        private HostedServiceHealthCheck? HostedServiceHealthCheck { get; }
        #endregion

        #region Constructor

        public ApplicationServerIotServices(IServiceProvider serviceProvider, HostedServiceHealthCheck hostedServiceHealthCheck)
		{
            this.ServiceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
            this.WSMessageManager = serviceProvider.GetService<IWSMessageManager>();
            this.HostedServiceHealthCheck = hostedServiceHealthCheck;
        }

        #endregion

        #region Methods     

        public async Task ReadStatusAsync(string data)
        {
            Log.Information("ApplicationServerIotServices.ReadStatusAsync");

            try
            {
                if (this.ServiceScopeFactory != null)
                {
                    using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                    {
                        ISupervisorServerIotStatus supervisorServerIotStatus = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryServerIotStatus>().CreateSupervisor();
                        SystemStatus? status = JsonSerializer.Deserialize<SystemStatus>(data);
                        if (status != null)
                        {
                            if (this.HostedServiceHealthCheck != null)
                            {
                                status.Date = Clock.Now;
                                this.HostedServiceHealthCheck.SetStatus(status);

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
