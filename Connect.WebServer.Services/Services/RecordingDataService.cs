using Connect.Data;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class RecordingDataService : CronScheduledService
    {
        #region Properties 

        //Every 10 minutes
        protected override string Schedule => "*/10 * * * *";

        #endregion

        #region Constructor

        /// <summary>
        /// Recording the operating data in database
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public RecordingDataService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        #endregion

        #region Methods

        public override async Task ProcessInScope(IServiceScope scope)
        {
            Log.Information("RecordingDataService");

            try
            {
                await this.AddOperatingDataForRoom(scope);
                await this.AddOperatingDataForConnectedObject(scope);                
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        private async Task AddOperatingDataForRoom(IServiceScope scope)
        {
            int roomCount = 0;

            ISupervisorOperatingData supervisorOperatingData = scope.ServiceProvider.GetRequiredService<ISupervisorOperatingData>();
            ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorRoom>();
            IEnumerable<Room> rooms = await supervisorRoom.GetRooms();
            foreach (Room room in rooms)
            {
                OperatingData dataRoom = new OperatingData()
                {
                    RoomId = room.Id,
                    Temperature = room.Temperature,
                    Humidity = room.Humidity,
                    Pressure = room.Pressure,
                    ConnectedObjectId = null,
                };

                if ((room.Temperature != null) || (room.Humidity != null) || (room.Pressure != null))
                {
                    if (await supervisorOperatingData.AddOperatingData(dataRoom) > 0)
                    {
                        roomCount++;
                    }
                }
            }
            Log.Information("RecordingDataService - rooms : " + roomCount);
        }

        private async Task AddOperatingDataForConnectedObject(IServiceScope scope)
        {
            int objectCount = 0;

            ISupervisorOperatingData supervisorOperatingData = scope.ServiceProvider.GetRequiredService<ISupervisorOperatingData>();
            ISupervisorConnectedObject supervisorConnectedObject = scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>();
            IEnumerable<ConnectedObject> objects = await supervisorConnectedObject.GetConnectedObjects();
            foreach (ConnectedObject obj in objects)
            {
                OperatingData dataObject = new OperatingData()
                {
                    ConnectedObjectId = obj.Id,
                    PlugOrder = obj.Plug?.Order,
                    PlugStatus = obj.Plug?.Status,
                    RoomId = obj.RoomId,
                    WorkingDuration = obj.Plug?.WorkingDuration,
                    Temperature = obj.Sensor?.Temperature,
                    Humidity = obj.Sensor?.Humidity,
                    Pressure = obj.Sensor?.Pressure,
                };

                if (await supervisorOperatingData.AddOperatingData(dataObject) > 0)
                {
                    objectCount++;
                }
            }
            Log.Information("RecordingDataService - connectedobjects : " + objectCount);
        }

        #endregion
    }
}
