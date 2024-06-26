﻿using Connect.Application;
using Connect.Data;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class CommandStatusService : ScheduledService
    {
        #region Properties
        #endregion

        #region Constructor

        public CommandStatusService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory, 10)
        {
        }

        #endregion

        #region Methods 
        /// <summary>
        /// Process the data in a scoped service that is launched at the startup of the app
        /// </summary>
        /// <returns></returns>
        public override async Task ProcessInScope(IServiceScope scope)
        {
            Log.Information("CommandStatusService.ProcessInScope");

            try
            {
                IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                ISupervisorPlug supervisorPlug = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryPlug>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
                ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryRoom>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
                IApplicationPlugServices applicationPlugServices = scope.ServiceProvider.GetRequiredService<IApplicationPlugServices>();
                CommandStatus? status = await applicationPlugServices.ReceiveCommandStatus();
                if (status != null)
                {
                    await ProcessPlugStatus(supervisorPlug, supervisorRoom, applicationPlugServices, status);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        private async Task ProcessPlugStatus(ISupervisorPlug supervisorPlug,
                                                ISupervisorRoom supervisorRoom,
                                                IApplicationPlugServices applicationPlugServices,
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
                    await applicationPlugServices.SendStatusToClientAsync(room.LocationId, plug);
                    await supervisorPlug.UpdatePlug(plug);
                }
            }
        }
        #endregion
    }
}
