﻿using Connect.Application.Infrastructure;
using Connect.Data;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal class ApplicationPlugServices : IApplicationPlugServices
	{
        #region Services
        private IAlertService? AlertService { get; }
        private IPlugService? PlugService { get; }
		private ISignalRConnectService? SignalRConnectService { get; }
        private IServiceScopeFactory? ServiceScopeFactory { get; }
        private IWSMessageManager? WSMessageManager { get; }
        #endregion

        #region Constructor
        public ApplicationPlugServices(IServiceProvider serviceProvider)
		{
			this.PlugService = serviceProvider.GetService<IPlugService>();
			this.SignalRConnectService = serviceProvider.GetService<ISignalRConnectService>();
            this.AlertService = AlertServiceFactory.CreateAlerteService(serviceProvider, ServiceAlertType.Mail);
			this.ServiceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
			this.WSMessageManager = serviceProvider.GetService<IWSMessageManager>();
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

        public async Task ReadStatusAsync(string data)
        {
            Log.Information("ApplicationPlugServices.ReadStatusAsync");

            try
            {
				if (this.ServiceScopeFactory != null)
				{
					using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
					{
						IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
						ISupervisorPlug supervisorPlug = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryPlug>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
						ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryRoom>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
						CommandStatus? status = status = JsonSerializer.Deserialize<CommandStatus>(data);
						if (status != null)
						{
							await this.ProcessPlugStatus(supervisorPlug, supervisorRoom, status);
						}
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
        public async Task<int> SendCommandAsync(Plug plug)
		{
			string? command = null;
			int res = -1;

			if (this.WSMessageManager != null)
			{
				if ((plug.Type & DeviceType.Outlet) == DeviceType.Outlet) //Prise
				{
					command = (plug.Order == Order.On) ? Command.PLUG_ON : Command.PLUG_OFF;
				}
				else if ((plug.Type & DeviceType.Module) == DeviceType.Module) //Module
				{
					command = (plug.Order == Order.On) ? Command.PLUG_OFF : Command.PLUG_ON;
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
				await this.WSMessageManager.SendMessageToAllAsync(json);
				Log.Information("SendCommandAsync - plug Id : " + plug.Id);
			}
			return res;
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

        private async Task ProcessPlugStatus(ISupervisorPlug supervisorPlug,
                                                ISupervisorRoom supervisorRoom,
                                                CommandStatus status)
        {
            Plug plug = await supervisorPlug.GetPlug(status.Address, status.Unit);
            if (plug != null)
            {
                plug.UpdateStatus(status.Status);
                Room room = await supervisorRoom.GetRoomFromPlugId(plug.Id);
                if (room != null)
                {
                    //Send Status to the clients (Mobile, Web...)
                    await this.SendStatusToClientAsync(room.LocationId, plug);
                    await supervisorPlug.UpdatePlug(plug);
                }
            }
        }
        #endregion
    }
}
