using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Data.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.Data.Database
{
    public sealed class ConnectDatabaseService : DatabaseService
    {
        #region Properties
        #endregion

        #region Constructor
        public ConnectDatabaseService(IDataContextFactory dataContextFactory, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(dataContextFactory, serviceScopeFactory, configuration)
        {
        }
        #endregion

        #region Methods
        protected override async Task<bool> InitializeDataAsync()
        {
            bool res = true;
            using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
            {
                await this.InitializePlugsAsync(scope);
                await this.LoadCacheAsync(scope);
            }
            return res;
        }
        protected override async Task<bool> UpgradeDatabaseAsync()
        {
            bool res = true;
            using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
            {
                ISupervisorVersion supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorVersion>();
                Version dbVersion = await supervisor.GetVersion();
                Log.Information($"dbVersion : {dbVersion}");

                if (this.Configuration != null)
                {
                    Version softwareVersion = new Version(this.Configuration["Version"] ?? "0.0.0");
                    Log.Information($"softwareVersion : {softwareVersion}");
                    if (softwareVersion > dbVersion)
                    {
                        if ((softwareVersion.CompareTo(new Version(0, 0, 0)) > 0) && softwareVersion.CompareTo(new Version(1, 0, 0)) < 0)
                        {
                            ISupervisorServerIotStatus supervisorServerIotStatus = scope.ServiceProvider.GetRequiredService<ISupervisorServerIotStatus>();
                            ResultCode code = await supervisorServerIotStatus.CreateTable();
                        }

                        if (softwareVersion.CompareTo(new Version(1, 0, 0)) > 0)
                        {
                            ISupervisorPlug supervisorPlug = scope.ServiceProvider.GetRequiredService<ISupervisorPlug>();
                            ResultCode code = await supervisorPlug.Upgrade1_1();
                        }
                    }
                    ResultCode result = await supervisor.UpdateVersion(softwareVersion.Major, softwareVersion.Minor, softwareVersion.Build);
                    if (result == ResultCode.ItemNotFound)
                    {
                        result = await supervisor.AddVersion();
                    }
                    res = (result == ResultCode.Ok) ? true : false;
                }
            }
            return res;
        }
        protected override async Task FeedDataAsync()
        {
            #region Location

            Location location = new Location()
            {
                Id = Guid.NewGuid().ToString(),
                Address = "10, rue Walter d'Islou",
                City = "Houilles",
                Country = "France",
                Zipcode = "78800",
                Description = "Maison",
            };

            #endregion

            #region Room

            Room livingRoom = new Room()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Cuisine - Séjour",
                Description = string.Empty,
                LocationId = location.Id,
                Type = RoomType.Kitchen,
                DeviceType = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity + DeviceType.Module,
            };

            Room bathRoom = new Room()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Salle de bain",
                Description = string.Empty,
                LocationId = location.Id,
                Type = RoomType.Bathroom,
                DeviceType = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity + DeviceType.Outlet,
            };

            Room bedRoom = new Room()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Chambre",
                Description = string.Empty,
                LocationId = location.Id,
                Type = RoomType.Bedroom,
                DeviceType = DeviceType.Module + DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity,
            };

            Room smallBedRoom = new Room()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Petite chambre",
                Description = string.Empty,
                LocationId = location.Id,
                Type = RoomType.SmallBedroom,
                DeviceType = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity,
            };

            #endregion

            #region Configuration

            Model.Configuration configA1 = new Model.Configuration()
            {
                Id = Guid.NewGuid().ToString(),
                ProtocolType = ConnectConstants.PROTOCOL_TYPE,
                Address = "374231",
                Period = 253,
                Pin0 = 10,
                Unit = "0"
            };

            Model.Configuration configA2 = new Model.Configuration()
            {
                Id = Guid.NewGuid().ToString(),
                ProtocolType = ConnectConstants.PROTOCOL_TYPE,
                Address = "28109046",
                Period = 255,
                Pin0 = 10,
                Unit = "1"
            };

            Model.Configuration configA3 = new Model.Configuration()
            {
                Id = Guid.NewGuid().ToString(),
                ProtocolType = ConnectConstants.PROTOCOL_TYPE,
                Address = "28109046",
                Period = 255,
                Pin0 = 10,
                Unit = "2"
            };

            Model.Configuration configA4 = new Model.Configuration()
            {
                Id = Guid.NewGuid().ToString(),
                ProtocolType = ConnectConstants.PROTOCOL_TYPE,
                Address = "28109046",
                Period = 255,
                Pin0 = 10,
                Unit = "3"
            };

            Model.Configuration configB1 = new Model.Configuration()
            {
                Id = Guid.NewGuid().ToString(),
                ProtocolType = ConnectConstants.PROTOCOL_TYPE,
                Address = "28109046",
                Period = 255,
                Pin0 = 10,
                Unit = "4"
            };

            Model.Configuration configD1 = new Model.Configuration()
            {
                Id = Guid.NewGuid().ToString(),
                ProtocolType = ConnectConstants.PROTOCOL_TYPE,
                Address = "28109046",
                Period = 255,
                Pin0 = 10,
                Unit = "12"
            };

            #endregion

            #region Sensor

            //Séjour (Non connecté)
            Sensor sensor1 = new Sensor()
            {
                Id = Guid.NewGuid().ToString(),
                Type = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity,
                RoomId = livingRoom.Id,
                ConnectedObjectId = null,
                Name = ConnectConstants.Type_F007th,
                Channel = "6",
                WorkingDuration = 0,
                Parameter = this.Configuration["SensorPeriod"] ?? "120000",
                OffSetHumidity = -10,
                OffSetTemperature = 0,
                IpAddress = ConnectConstants.ArduinoServer,
            };

            //Petite chambre
            Sensor sensor2 = new Sensor()
            {
                Id = Guid.NewGuid().ToString(),
                Type = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity,
                RoomId = smallBedRoom.Id,
                ConnectedObjectId = null,
                Name = ConnectConstants.Type_HTUD21DF,
                Channel = "1",
                WorkingDuration = 0,
                Parameter = this.Configuration["SensorPeriod"] ?? "120000",
                OffSetHumidity = +4,
                OffSetTemperature = 0.4f,
                IpAddress = ConnectConstants.ArduinoServer,
            };

            //Congélateur
            Sensor sensor3 = new Sensor()
            {
                Id = Guid.NewGuid().ToString(),
                Type = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity,
                RoomId = string.Empty,
                Name = ConnectConstants.Type_F007th,
                Channel = "1",
                WorkingDuration = 0,
                Parameter = this.Configuration["SensorPeriod"] ?? "120000",
                OffSetHumidity = -10,
                OffSetTemperature = 0,
                IpAddress = ConnectConstants.ArduinoServer,
            };

            //Salle de bain
            Sensor sensor4 = new Sensor()
            {
                Id = Guid.NewGuid().ToString(),
                Type = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity,
                RoomId = bathRoom.Id,
                ConnectedObjectId = null,
                Name = ConnectConstants.Type_F007th,
                Channel = "5",
                WorkingDuration = 0,
                Parameter = this.Configuration["SensorPeriod"] ?? "120000",
                OffSetHumidity = -10,
                OffSetTemperature = 0,
                IpAddress = ConnectConstants.ArduinoServer,
            };

            //Chambre
            Sensor sensor5 = new Sensor()
            {
                Id = Guid.NewGuid().ToString(),
                Type = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity,
                RoomId = bedRoom.Id,
                Name = ConnectConstants.Type_F007th,
                Channel = "2",
                WorkingDuration = 0,
                Parameter = this.Configuration["SensorPeriod"] ?? "120000",
                OffSetHumidity = -10,
                OffSetTemperature = 0,
                IpAddress = ConnectConstants.ArduinoServer,
            };

            //Séjour
            Sensor sensor6 = new Sensor()
            {
                Id = Guid.NewGuid().ToString(),
                Type = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity,
                RoomId = livingRoom.Id,
                Name = ConnectConstants.Type_F007th,
                Channel = "3",
                WorkingDuration = 0,
                Parameter = this.Configuration["SensorPeriod"] ?? "120000",
                OffSetHumidity = -10,
                OffSetTemperature = 0,
                IpAddress = ConnectConstants.ArduinoServer,
            };

            //Frigo
            Sensor sensor7 = new Sensor()
            {
                Id = Guid.NewGuid().ToString(),
                Type = DeviceType.Sensor_Temperature + DeviceType.Sensor_Humidity,
                RoomId = string.Empty,
                Name = ConnectConstants.Type_F007th,
                Channel = "4",
                WorkingDuration = 0,
                Parameter = this.Configuration["SensorPeriod"] ?? "120000",
                OffSetHumidity = -10,
                OffSetTemperature = 0,
                IpAddress = ConnectConstants.ArduinoServer,
            };

            //Séjour-Cuisine
            Sensor sensor8 = new Sensor()
            {
                Id = Guid.NewGuid().ToString(),
                Type = DeviceType.Sensor_Water_Leak,
                RoomId = livingRoom.Id,
                Name = ConnectConstants.Type_MC22_1527,
                Channel = "1",
                Parameter = "8898105",
                WorkingDuration = 0,
                OffSetHumidity = 0,
                OffSetTemperature = 0,
                IpAddress = ConnectConstants.ArduinoServer,
            };

            //Salle de bain
            Sensor sensor9 = new Sensor()
            {
                Id = Guid.NewGuid().ToString(),
                Type = DeviceType.Sensor_Water_Leak,
                RoomId = bathRoom.Id,
                Name = ConnectConstants.Type_MC22_1527,
                Channel = "2",
                Parameter = "9116489",
                WorkingDuration = 0,
                OffSetHumidity = 0,
                OffSetTemperature = 0,
                IpAddress = ConnectConstants.ArduinoServer,
            };

            #endregion

            #region Plug

            Plug plugA1 = new Plug()
            {
                Id = Guid.NewGuid().ToString(),
                Mode = Mode.Manual,
                Name = "Module connecté",
                RoomId = string.Empty,
                Order = Order.Off,
                ConfigurationId = configA1.Id,
                Type = DeviceType.Module,
                LastDateTimeOn = null,
                WorkingDuration = 0,
                ConditionType = ParameterType.Temperature,
            };

            Plug plug1 = new Plug()
            {
                Id = Guid.NewGuid().ToString(),
                Mode = Mode.Manual,
                Name = "Prise connectée",
                Order = Order.Off,
                RoomId = string.Empty,
                ConfigurationId = configA3.Id,
                Type = DeviceType.Outlet,
                LastDateTimeOn = null,
                WorkingDuration = 0,
                ConditionType = ParameterType.Temperature,
            };

            Plug plug2 = new Plug()
            {
                Id = Guid.NewGuid().ToString(),
                Mode = Mode.Manual,
                Name = "Prise connectée",
                Order = Order.Off,
                RoomId = string.Empty,
                ConfigurationId = configA4.Id,
                Type = DeviceType.Outlet,
                LastDateTimeOn = null,
                WorkingDuration = 0,
                ConditionType = ParameterType.Humidity,
            };

            Plug plugA2 = new Plug()
            {
                Id = Guid.NewGuid().ToString(),
                Mode = Mode.Manual,
                Name = "Module connecté",
                Order = Order.Off,
                RoomId = string.Empty,
                ConfigurationId = configA2.Id,
                Type = DeviceType.Module,
                LastDateTimeOn = null,
                WorkingDuration = 0,
                ConditionType = ParameterType.Temperature,
            };

            #endregion

            #region ConnectedObject            

            ConnectedObject obj1 = new ConnectedObject()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Congélateur",
                RoomId = livingRoom.Id,
                DeviceType = DeviceType.Sensor_Temperature,
            };

            sensor3.ConnectedObjectId = obj1.Id;

            ConnectedObject obj2 = new ConnectedObject()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Chauffage fenêtre",
                RoomId = livingRoom.Id,
                DeviceType = DeviceType.Module,
            };

            plugA1.ConnectedObjectId = obj2.Id;

            ConnectedObject obj3 = new ConnectedObject()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sèche serviette",
                RoomId = bathRoom.Id,
                DeviceType = DeviceType.Outlet,
            };

            plug1.ConnectedObjectId = obj3.Id;

            ConnectedObject obj4 = new ConnectedObject()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Ventilation salle de bain",
                RoomId = bathRoom.Id,
                DeviceType = DeviceType.Outlet,
            };

            plug2.ConnectedObjectId = obj4.Id;

            ConnectedObject obj5 = new ConnectedObject()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Chauffage mur cuisine",
                RoomId = livingRoom.Id,
                DeviceType = DeviceType.Module,
            };

            plugA2.ConnectedObjectId = obj5.Id;

            ConnectedObject obj8 = new ConnectedObject()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Réfrigérateur",
                RoomId = livingRoom.Id,
                DeviceType = DeviceType.Sensor_Temperature,
            };

            sensor7.ConnectedObjectId = obj8.Id;

            #endregion

            #region Program

            Model.Program program1 = new Model.Program()
            {
                Id = Guid.NewGuid().ToString(),
                ConnectedObjectId = plug1.Id,
            };

            plug1.ProgramId = program1.Id;

            Model.Program program2 = new Model.Program()
            {
                Id = Guid.NewGuid().ToString(),
                ConnectedObjectId = plug2.Id,
            };

            plug2.ProgramId = program2.Id;

            Model.Program programA1 = new Model.Program()
            {
                Id = Guid.NewGuid().ToString(),
                ConnectedObjectId = plugA1.Id,
            };

            plugA1.ProgramId = programA1.Id;

            Model.Program programA2 = new Model.Program()
            {
                Id = Guid.NewGuid().ToString(),
                ConnectedObjectId = plugA2.Id,
            };

            plugA2.ProgramId = programA2.Id;

            #endregion

            #region Condition

            Condition condition1 = new Condition()
            {
                Id = Guid.NewGuid().ToString(),
                TemperatureOrder = null,
                HumidityOrder = null,
                OperationRangetId = null,
                TemperatureOrderIsEnabled = 0,
                HumidityOrderIsEnabled = 0,
            };

            plug1.ConditionId = condition1.Id;

            Condition condition2 = new Condition()
            {
                Id = Guid.NewGuid().ToString(),
                TemperatureOrder = null,
                HumidityOrder = null,
                OperationRangetId = null,
                TemperatureOrderIsEnabled = 0,
                HumidityOrderIsEnabled = 0,
            };

            plug2.ConditionId = condition2.Id;

            Condition conditionA1 = new Condition()
            {
                Id = Guid.NewGuid().ToString(),
                TemperatureOrder = null,
                HumidityOrder = null,
                OperationRangetId = null,
                TemperatureOrderIsEnabled = 0,
                HumidityOrderIsEnabled = 0,
            };

            plugA1.ConditionId = conditionA1.Id;

            Condition conditionA2 = new Condition()
            {
                Id = Guid.NewGuid().ToString(),
                TemperatureOrder = null,
                HumidityOrder = null,
                OperationRangetId = null,
                TemperatureOrderIsEnabled = 0,
                HumidityOrderIsEnabled = 0,
            };

            plugA2.ConditionId = conditionA2.Id;

            #endregion

            using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
            {
                await scope.ServiceProvider.GetRequiredService<ISupervisorVersion>().AddVersion();
                await scope.ServiceProvider.GetRequiredService<ISupervisorLocation>().AddLocation(location);
                await scope.ServiceProvider.GetRequiredService<ISupervisorRoom>().AddRoom(livingRoom);
                await scope.ServiceProvider.GetRequiredService<ISupervisorRoom>().AddRoom(bathRoom);
                await scope.ServiceProvider.GetRequiredService<ISupervisorRoom>().AddRoom(bedRoom);
                await scope.ServiceProvider.GetRequiredService<ISupervisorRoom>().AddRoom(smallBedRoom);
                await scope.ServiceProvider.GetRequiredService<ISupervisorPlug>().AddPlug(plug1);
                await scope.ServiceProvider.GetRequiredService<ISupervisorPlug>().AddPlug(plug2);
                await scope.ServiceProvider.GetRequiredService<ISupervisorPlug>().AddPlug(plugA1);
                await scope.ServiceProvider.GetRequiredService<ISupervisorPlug>().AddPlug(plugA2);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConfiguration>().AddConfiguration(configA1);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConfiguration>().AddConfiguration(configA2);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConfiguration>().AddConfiguration(configA3);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConfiguration>().AddConfiguration(configA4);
                await scope.ServiceProvider.GetRequiredService<ISupervisorCondition>().AddCondition(condition1);
                await scope.ServiceProvider.GetRequiredService<ISupervisorCondition>().AddCondition(condition2);
                await scope.ServiceProvider.GetRequiredService<ISupervisorCondition>().AddCondition(conditionA1);
                await scope.ServiceProvider.GetRequiredService<ISupervisorCondition>().AddCondition(conditionA2);
                await scope.ServiceProvider.GetRequiredService<ISupervisorProgram>().AddProgram(program1);
                await scope.ServiceProvider.GetRequiredService<ISupervisorProgram>().AddProgram(program2);
                await scope.ServiceProvider.GetRequiredService<ISupervisorProgram>().AddProgram(programA1);
                await scope.ServiceProvider.GetRequiredService<ISupervisorProgram>().AddProgram(programA2);
                // await this.Supervisor.AddSensor(sensor1);
                await scope.ServiceProvider.GetRequiredService<ISupervisorSensor>().AddSensor(sensor2);
                await scope.ServiceProvider.GetRequiredService<ISupervisorSensor>().AddSensor(sensor3);
                await scope.ServiceProvider.GetRequiredService<ISupervisorSensor>().AddSensor(sensor4);
                await scope.ServiceProvider.GetRequiredService<ISupervisorSensor>().AddSensor(sensor5);
                await scope.ServiceProvider.GetRequiredService<ISupervisorSensor>().AddSensor(sensor6);
                await scope.ServiceProvider.GetRequiredService<ISupervisorSensor>().AddSensor(sensor7);
                await scope.ServiceProvider.GetRequiredService<ISupervisorSensor>().AddSensor(sensor8);
                await scope.ServiceProvider.GetRequiredService<ISupervisorSensor>().AddSensor(sensor9);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>().AddConnectedObject(obj1);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>().AddConnectedObject(obj2);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>().AddConnectedObject(obj3);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>().AddConnectedObject(obj4);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>().AddConnectedObject(obj5);
                await scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>().AddConnectedObject(obj8);
            }
        }
        private async Task InitializePlugsAsync(IServiceScope scope)
        {
            ISupervisorPlug supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorPlug>();
            List<Plug> plugs = (await supervisor.GetPlugs()).ToList();
            foreach (Plug plug in plugs)
            {
                plug.WorkingDuration = 0;

                if (plug.Status is Status.ON or Status.OffON)
                {
                    plug.LastDateTimeOn = DateTime.Now;
                }
                else
                {
                    plug.LastDateTimeOn = null;
                }

                await supervisor.UpdatePlug(plug);
            }
        }

        private async Task LoadCacheAsync(IServiceScope scope)
        {
            ISupervisorCacheCondition supervisorCacheCondition = scope.ServiceProvider.GetRequiredService<ISupervisorCacheCondition>();
            await supervisorCacheCondition.Initialize();

            ISupervisorCacheConfiguration supervisorCacheConfiguration = scope.ServiceProvider.GetRequiredService<ISupervisorCacheConfiguration>();
            await supervisorCacheConfiguration.Initialize();

            ISupervisorCacheConnectedObject supervisorCacheConnectedObject = scope.ServiceProvider.GetRequiredService<ISupervisorCacheConnectedObject>();   
            await supervisorCacheConnectedObject.Initialize();

            ISupervisorCacheLocation supervisorCacheLocation = scope.ServiceProvider.GetRequiredService<ISupervisorCacheLocation>();
            await supervisorCacheLocation.Initialize();

            ISupervisorCacheNotification supervisorCacheNotification = scope.ServiceProvider.GetRequiredService<ISupervisorCacheNotification>();
            await supervisorCacheNotification.Initialize();

            ISupervisorCacheOperationRange supervisorCacheOperationRange = scope.ServiceProvider.GetRequiredService<ISupervisorCacheOperationRange>();
            await supervisorCacheOperationRange.Initialize();

            ISupervisorCachePlug supervisorCachePlug = scope.ServiceProvider.GetRequiredService<ISupervisorCachePlug>();
            await supervisorCachePlug.Initialize(); 

            ISupervisorCacheProgram supervisorCacheProgram = scope.ServiceProvider.GetRequiredService<ISupervisorCacheProgram>();
            await supervisorCacheProgram.Initialize();

            ISupervisorCacheRoom supervisorCacheRoom = scope.ServiceProvider.GetRequiredService<ISupervisorCacheRoom>();
            await supervisorCacheRoom.Initialize();

            ISupervisorCacheSensor supervisorCacheSensor = scope.ServiceProvider.GetRequiredService<ISupervisorCacheSensor>();
            await supervisorCacheSensor.Initialize();   
        }
        #endregion
    }
}
