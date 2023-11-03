using Connect.Application;
using Connect.Data;
using Connect.Model;
using Framework.Infrastructure.Services;
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
        /// Process the data in a scoped service which is launched at the startup of the app
        /// </summary>
        /// <returns></returns>
        public override async Task ProcessInScope(IServiceScope scope)
        {
            Log.Information("CommandStatusService.ProcessInScope");

            try
            {
                ISupervisorPlug supervisorPlug = scope.ServiceProvider.GetRequiredService<ISupervisorPlug>();
                IApplicationPlugServices applicationPlugServices = scope.ServiceProvider.GetRequiredService<IApplicationPlugServices>();

                CommandStatus? status = await applicationPlugServices.ReceiveCommandStatus();
                if (status != null)
                {
                    Plug plug = await supervisorPlug.GetPlug(status.Address, status.Unit);
                    if (plug != null)
                    {
                        ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorRoom>();
                        plug.UpdateStatus(status.Status);
                        Room room = await supervisorRoom.GetRoomFromPlugId(plug.Id);
                        if (room != null)
                        {
                            //Send Status to the clients (Mobile, Web...)
                            await applicationPlugServices.SendStatusToClientAsync(room.LocationId, plug);
                            await supervisorPlug.UpdatePlug(plug);

                            if (plug.ConnectedObjectId != null)
                            {
                                ISupervisorConnectedObject supervisorConnectedObject = scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>();
                                ConnectedObject connectedObject = await supervisorConnectedObject.GetConnectedObject(plug.ConnectedObjectId, false);
                                if (connectedObject != null)
                                {
                                    //Send Notification to the Firebase app
                                    await applicationPlugServices.NotifyPlugStatus(room.LocationId, plug, connectedObject.Name);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        #endregion
    }
}
