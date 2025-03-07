﻿using AirZapto.Data.Services;
using AirZapto.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data.Supervisors.Tests
{
    public class SupervisorSensorDataTest : SupervisorBase
    {
        [Fact]
        public async Task AddSensorData()
        {
            //Arrange
            await this.Initialyse();

            //Act
            ISupervisorSensorData supervisor = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensorData>().CreateSupervisor();
            ResultCode code = await supervisor.AddSensorDataAsync(new Model.AirZaptoData()
            {
                CO2 = 500,
                Temperature = 10.2f,
                SensorId = "1"
            });

            //Assert
            Assert.True(code == ResultCode.Ok);
        }

        [Fact]
        public async Task GetSensorData()
        {
            //Arrange
            await this.Initialyse();
            (ResultCode code, Sensor? sensor) obj1 = await this.CreateSensor();

            //Act
            ISupervisorSensorData? supervisorSensorData = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensorData>().CreateSupervisor();
            (ResultCode code, IEnumerable<Model.AirZaptoData>? data) obj2 = await supervisorSensorData!.GetSensorDataAsync(obj1.sensor!.Id, 60);

            //Assert
            Assert.True(obj2.code == ResultCode.Ok);
            Assert.True(obj2.data!.ToList().Exists((item) => item.SensorId == obj1.sensor.Id));
        }

        [Fact]
        public async Task DeleteSensorData()
        {
            //Arrange
            await this.Initialyse();
            (ResultCode code, Sensor? sensor) obj1 = await this.CreateSensor();

            //Act
            ISupervisorSensorData? supervisorSensorData = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensorData>().CreateSupervisor();
            ResultCode code = await supervisorSensorData.DeleteSensorDataAsync(new TimeSpan(0, 0, 0));
            if (code == ResultCode.Ok) 
            {
                (ResultCode code, IEnumerable<Model.AirZaptoData>? data) obj2 = await supervisorSensorData!.GetSensorDataAsync(obj1.sensor!.Id, 60);
                //Assert
                Assert.True(obj2.code == ResultCode.ItemNotFound);
                Assert.False(obj2.data!.Any());
            }

            //Assert
            Assert.True(code == ResultCode.Ok);
        }

        [Fact]
        public async Task GetTimeLastSensorData()
        {
            //Arrange
            await this.Initialyse();
            (ResultCode code, Sensor? sensor) obj1 = await this.CreateSensor();

            //Act
            ISupervisorSensorData? supervisorSensorData = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensorData>().CreateSupervisor();
            DateTime? dateTime = await supervisorSensorData.GetTimeLastSensorDataAsync(obj1.sensor!.Id);

            //Assert
            Assert.NotNull(dateTime);
        }

        protected async Task<(ResultCode, Sensor?)> CreateSensor()
        {

            Sensor? sensor = null;
            ISupervisorSensorData? supervisorSensorData = null;
            ISupervisorSensor supervisorSensor = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor();
            ResultCode code = await supervisorSensor.AddUpdateSensorAsync(new Sensor()
            {
                Humidity = 10.5f,
                Temperature = 25.5f,
                Pressure = null,
                CO2 = 500,
                Period = "60",
                Channel = "1",
                IdSocket = "1",
                Port = 1000,
                IpAddress = "127.0.0.1",
                Name = "Sensor"
            });

            if (code == ResultCode.Ok)
            {
                (code, sensor) = await supervisorSensor.GetSensorFromIdSocketAsync("1");
                if (code == ResultCode.Ok)
                {
                    supervisorSensorData = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensorData>().CreateSupervisor();
                    code = await supervisorSensorData.AddSensorDataAsync(new Model.AirZaptoData()
                    {
                        CO2 = 500,
                        Temperature = 10.2f,
                        SensorId = sensor!.Id,
                    });
                }
            }
           
            return (code, sensor);
        }
    }
}
