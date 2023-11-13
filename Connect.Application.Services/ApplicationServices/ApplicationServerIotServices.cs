using Connect.Model;
using Framework.Core.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal class ApplicationServerIotServices : IApplicationServerIotServices
    {
		#region Services
		private IUdpCommunicationService? UdpCommunicationService { get; }
		#endregion

		#region Constructor

		public ApplicationServerIotServices(IServiceProvider serviceProvider)
		{
			this.UdpCommunicationService = serviceProvider.GetService<IUdpCommunicationService>();
		}

        #endregion

        #region Methods        

        public async Task<SystemStatus?> ReceiveStatusAsync()
        {
            SystemStatus? result = null;

            try
            {
                if (this.UdpCommunicationService != null)
                {
                    byte[] data = await this.UdpCommunicationService.StartReception(ConnectConstants.PortServerIotStatus);
                    if (data!.Length > 0)
                    {
                        result = JsonSerializer.Deserialize<SystemStatus>(Encoding.UTF8.GetString(data, 0, data.Length));
                        Log.Warning("ReceiveStatusAsync - SystemStatus : " + result?.IpAddress + "/" + result?.RSSI + "/" + result?.Status);
                    }
                    else
                    {
                        Log.Error("ReceiveStatusAsync - No data");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return result;
        }

        #endregion
    }
}
