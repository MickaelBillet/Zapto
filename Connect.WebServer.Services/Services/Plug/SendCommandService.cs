using Connect.Application;
using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public interface ISendCommandService
    {
        Task<ResultCode> Execute(Plug plug);
    }

    internal class SendCommandService : ISendCommandService
    {
        #region Services
        private ISupervisorRoom SupervisorRoom { get; }
        private IApplicationPlugServices ApplicationPlugServices { get; }
        private ISupervisorPlug SupervisorPlug { get; }
        #endregion

        #region Constructor
        public SendCommandService(IServiceProvider serviceProvider)
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            this.SupervisorRoom = serviceProvider.GetRequiredService<ISupervisorFactoryRoom>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
            this.SupervisorPlug = serviceProvider.GetRequiredService<ISupervisorFactoryPlug>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
            this.ApplicationPlugServices = serviceProvider.GetRequiredService<IApplicationPlugServices>();
        }
        #endregion

        #region Methods
        public async Task<ResultCode> Execute(Plug plug)
        {
            ResultCode resultCode = ResultCode.CouldNotUpdateItem;
            Room room = await this.SupervisorRoom.GetRoomFromPlugId(plug.Id);
            if (room != null)
            {
                //Get order before the update
                int previousOrder = plug.Order;

                //Update the order with all the data
                plug.UpdateOrder(room.Humidity, room.Temperature);

                //We send the command to the Arduino when the order changes or when the last command has not been received after one minute
                if ((plug.Order != previousOrder)
                    || ((Clock.Now - plug.LastCommandDateTime > new TimeSpan(0, 1, 0)) && (plug.CommandReceived == 0)))
                {
                    //Send Command to Arduino
                    if (await this.ApplicationPlugServices.SendCommand(plug) <= 0)
                    {
                        //If we cannot send the command to the Arduino, we keep the previous value
                        plug.Order = previousOrder;
                    }
                    else
                    {
                        plug.LastCommandDateTime = Clock.Now;
                        plug.CommandReceived = 0;
                    }
                }

                Log.Information("ProcessingDataService.ProcessInScope " + "Id : " + plug.Id);
                Log.Information("ProcessingDataService.ProcessInScope " + "OnOff : " + plug.OnOff);
                Log.Information("ProcessingDataService.ProcessInScope " + "Status : " + plug.Status);
                Log.Information("ProcessingDataService.ProcessInScope " + "Order : " + plug.Order);

                //Update the working duration
                plug.ComputeWorkingDuration();

                //Send Status to the clients app (Mobile, Web...)
                await this.ApplicationPlugServices.SendStatusToClient(room.LocationId, plug);
                resultCode = await this.SupervisorPlug.UpdatePlug(plug);
            }
            return resultCode;
        }
        #endregion
    }
}
