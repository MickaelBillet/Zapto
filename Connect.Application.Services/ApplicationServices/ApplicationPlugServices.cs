using Connect.Application.Infrastructure;
using Connect.Data;
using Connect.Model;
using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal sealed class ApplicationPlugServices : IApplicationPlugServices
	{
        #region Services
        private IAlertService? AlertService { get; }
        private IPlugService? PlugService { get; }
		private ISignalRConnectService? SignalRConnectService { get; }
        private IServiceScopeFactory? ServiceScopeFactory { get; }
		private ISendMessageToArduinoService? SendMessageToArduino { get; }
        #endregion

        #region Constructor
        public ApplicationPlugServices(IServiceProvider serviceProvider)
		{
			this.PlugService = serviceProvider.GetService<IPlugService>();
			this.SignalRConnectService = serviceProvider.GetService<ISignalRConnectService>();
            this.AlertService = AlertServiceFactory.CreateAlerteService(serviceProvider, ServiceAlertType.Mail);
			this.SendMessageToArduino = serviceProvider.GetService<ISendMessageToArduinoService>();
			this.ServiceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
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

        public async Task ReadStatus(CommandStatus? status)
        {
            Log.Information("ApplicationPlugServices.ReadStatus");

            try
            {
				if (this.ServiceScopeFactory != null)
				{
					using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
					{
						IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
						ISupervisorPlug supervisorPlug = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryPlug>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
						ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryRoom>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
						await this.ProcessPlugStatus(supervisorPlug, supervisorRoom, status);
                    }
				}
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        /// <summary>
        /// Send the plug command to the iot server
        /// </summary>
        public async Task<int> SendCommand(Plug plug)
		{
			string? command = null;
			int res = -1;

			if (this.SendMessageToArduino != null)
			{
				if ((plug.Type & DeviceType.Outlet) == DeviceType.Outlet) //Prise
				{
					command = (plug.Order == Order.On) ? Command.PLUG_ON : Command.PLUG_OFF;
				}
				else if ((plug.Type & DeviceType.Module) == DeviceType.Module) //Module
				{
					command = (plug.Order == Order.On) ? Command.PLUG_OFF : Command.PLUG_ON;
				}

				string? json = plug.SerializePlugCommand(command);
				if (string.IsNullOrEmpty(json) == false)
				{
					res = await this.SendMessageToArduino.Send(MessageArduino.Serialize(new MessageArduino()
                    {
                        Header = ConnectConstants.PlugCommand,
                        Payload = json
                    }));

					Log.Information("SendCommandAsync - plug Id : " + plug.Id);
					Log.Information("SendCommandAsync - res : " + res);
				}
			}
			return res;
		}

		/// <summary>
		/// Send the plug status to the clients
		/// </summary>
		/// <param name="locationId"></param>
		/// <param name="plug"></param>
		/// <returns></returns>
		public async Task SendStatusToClient(string locationId, Plug plug)
		{
			if (this.SignalRConnectService != null)
			{
				await this.SignalRConnectService.SendPlugStatusAsync(locationId, plug);
			}
		}

		/// <summary>
		/// Notify the plug status
		/// </summary>
		/// <param name="locationId"></param>
		/// <param name="plug"></param>
		/// <param name="nameConnectedObject"></param>
		/// <returns></returns>
        public async Task NotifyPlugStatus(string locationId, Plug plug, string nameConnectedObject)
        {
			if (this.AlertService != null)
			{
				if ((plug.Order == Order.Off) && (plug.Status == Status.ON))
				{
					await this.AlertService.SendAlertAsync(locationId, nameConnectedObject, "Allumage manuel");
                }
                else if ((plug.Order == Order.On) && (plug.Status == Status.OFF))
				{
                    await this.AlertService.SendAlertAsync(locationId, nameConnectedObject, "Coupure manuelle");
                }
            }
        }

        private async Task ProcessPlugStatus(ISupervisorPlug supervisorPlug, ISupervisorRoom supervisorRoom, CommandStatus? status)
        {
			if (status != null)
			{
				Plug plug = await supervisorPlug.GetPlug(status.Address, status.Unit);
				if (plug != null)
				{
					plug.UpdateStatus(status.Status);
					Room room = await supervisorRoom.GetRoomFromPlugId(plug.Id);
					if (room != null)
					{
						//Send Status to the clients (Mobile, Web...)
						await this.SendStatusToClient(room.LocationId, plug);
						await supervisorPlug.UpdatePlug(plug);
					}
				}
			}
        }
        #endregion
    }
}
