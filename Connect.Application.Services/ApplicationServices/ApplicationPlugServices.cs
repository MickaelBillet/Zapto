using Connect.Application.Infrastructure;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal class ApplicationPlugServices : IApplicationPlugServices
	{
        #region Services
        private IAlertService? AlertService { get; }
        private IPlugService? PlugService { get; }
		private IUdpCommunicationService? UdpCommunicationService { get; }
		private ISignalRConnectService? SignalRConnectService { get; }
		#endregion

		#region Constructor

		public ApplicationPlugServices(IServiceProvider serviceProvider)
		{
			this.PlugService = serviceProvider.GetService<IPlugService>();
			this.SignalRConnectService = serviceProvider.GetService<ISignalRConnectService>();
			this.UdpCommunicationService = serviceProvider.GetService<IUdpCommunicationService>();
            this.AlertService = AlertServiceFactory.CreateAlerteService(serviceProvider, ServiceAlertType.Mail);
        }
        #endregion

        #region Methods
        public async Task<bool?> ChangeMode(Plug plug)
		{
			return (this.PlugService != null) ? await this.PlugService.ManualProgrammingAsync(plug) : null;
		}

		public async Task<bool?> SwitchOnOff(Plug plug)
		{
			return (this.PlugService != null) ? await this.PlugService.OnOffAsync(plug) : null;
		}

		/// <summary>
		/// Send the plug command to the iot server
		/// </summary>
		public async Task<int> SendCommandAsync(Plug plug)
		{
			string? command = null;
			int res = -1;

			if (this.UdpCommunicationService != null)
			{
				if ((plug.Type & DeviceType.Outlet) == DeviceType.Outlet) //Prise
				{
					command = (plug.Order == 1) ? Command.PLUG_ON : Command.PLUG_OFF;
				}
				else if ((plug.Type & DeviceType.Module) == DeviceType.Module) //Module
				{
					command = (plug.Order == 1) ? Command.PLUG_OFF : Command.PLUG_ON;
				}

				PlugCommand plugCommand = new PlugCommand()
				{
					ProtocolType = plug.Configuration?.ProtocolType,
					Address = plug.Configuration?.Address,
					Unit = plug.Configuration?.Unit,
					Pin0 = plug.Configuration?.Pin0.ToString(),
					Period = plug.Configuration?.Period.ToString(),
					Order = command,
				};

				string json = JsonSerializer.Serialize<PlugCommand>(plugCommand);
				res = await this.UdpCommunicationService.SendMessage(json, ConnectConstants.PortPlugCommand, ConnectConstants.ArduinoServer);
				Log.Information("SendCommand - plug Id : " + plug.Id);
				Log.Information("SendCommand - size data : " + res);
			}
			return res;
		}

		/// <summary>
		/// Receive the command status plug from the iot server
		/// </summary>
		/// <returns></returns>
		public async Task<CommandStatus?> ReceiveCommandStatus()
		{
			CommandStatus? status = null;
			if (this.UdpCommunicationService != null)
			{
				byte[] data = await this.UdpCommunicationService.StartReception(ConnectConstants.PortPlugStatus);

				if (data?.Length > 0)
				{
					status = JsonSerializer.Deserialize<CommandStatus>(Encoding.UTF8.GetString(data, 0, data.Length));

					Log.Information("ReceiveCommandStatus - CommandStatus Address " + status?.Address + " Unit " + status?.Unit + " Status " + status?.Status);
				}
				else
				{
					Log.Warning("ReceiveCommandStatus - No data");
				}
			}
			return status;
		}

		/// <summary>
		/// Send the plug status to the clients
		/// </summary>
		/// <param name="locationId"></param>
		/// <param name="plug"></param>
		/// <returns></returns>
		public async Task SendStatusToClientAsync(string locationId, Plug plug)
		{
			if (this.SignalRConnectService != null)
			{
				await this.SignalRConnectService.SendPlugStatusAsync(locationId, plug);
			}
		}

		/// <summary>
		/// Notify the plug status to the firebase app
		/// </summary>
		/// <param name="locationId"></param>
		/// <param name="plug"></param>
		/// <param name="nameConnectedObject"></param>
		/// <returns></returns>
        public async Task NotifyPlugStatus(string locationId, Plug plug, string nameConnectedObject)
        {
			if (this.AlertService != null)
			{
				if ((plug.Order == 0) && (plug.Status == 1))
				{
					await this.AlertService.SendAlertAsync(locationId, nameConnectedObject, "Allumage manuel");
                }
                else if ((plug.Order == 1) && (plug.Status == 0))
				{
                    await this.AlertService.SendAlertAsync(locationId, nameConnectedObject, "Coupure manuelle");
                }
            }
        }
        #endregion
    }
}
