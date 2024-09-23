using AirZapto.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data.Supervisors.Tests
{
    public class SupervisorSensorTest : SupervisorBase
    {
        public SupervisorSensorTest() : base() 
        { 
        }


        [Fact]
        public async Task AddSensor()
        {
            // Arrange
            await this.Initialyse();

            //Act
            ISupervisorSensor supervisor = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor();
            ResultCode code = await supervisor.AddUpdateSensorAsync(new Sensor()
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

            //Assert
            Assert.True(code == ResultCode.Ok);
        }

        [Fact]
        public async Task GetSensors()
        {
            // Arrange
            await this.Initialyse();
            Sensor sensor = new Sensor()
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
            };

            //Act
            IEnumerable<Sensor>? sensors = null;
            ISupervisorSensor supervisor = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor();
            ResultCode code = await supervisor.AddUpdateSensorAsync(sensor);

            if (code == ResultCode.Ok) 
            {
                (code, sensors) = await supervisor.GetSensorsAsync();
            }

            //Assert
            Assert.True(code == ResultCode.Ok);
            Assert.True(sensors!.ToList().Exists((item) => item.IdSocket == sensor.IdSocket));
        }

        [Fact]
        public async Task GetSensorFromId()
        {
            // Arrange
            await this.Initialyse();

            //Act
            IEnumerable<Sensor>? sensors = null;
            ISupervisorSensor supervisor = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor();
            ResultCode code = await supervisor.AddUpdateSensorAsync(new Sensor()
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
                (code, sensors) = await supervisor.GetSensorsAsync();
            }

            //Assert
            Assert.True(code == ResultCode.Ok);

            Sensor? sensor = null;
            foreach (Sensor item in sensors!)
            {
                (code, sensor) = await supervisor.GetSensorAsync(item.Id);
                if ((code == ResultCode.Ok) && sensor != null)
                {
                    break;
                }
            }

            Assert.True(sensor != null);
        }

        [Fact]
        public async Task GetSensorFromIdSocket()
        {
            // Arrange
            await this.Initialyse();

            //Act
            ISupervisorSensor supervisor = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor();
            ResultCode code = await supervisor.AddUpdateSensorAsync(new Sensor()
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

            (ResultCode code, Sensor? sensor)? obj = await supervisor.GetSensorFromIdSocketAsync("1");

            //Assert
            Assert.True(code == ResultCode.Ok);
            Assert.True(obj?.sensor != null && obj?.code == ResultCode.Ok);
        }

        [Fact]
        public async Task UpdateSensor()
        {
            // Arrange
            await this.Initialyse();
            Sensor? sensor = new Sensor()
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
            };

            //Act
            IEnumerable<Sensor>? sensors = null;
            ISupervisorSensor supervisor = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor();
            ResultCode code = await supervisor.AddUpdateSensorAsync(sensor);

            if (code == ResultCode.Ok)
            {
                (code, sensors) = await supervisor.GetSensorsAsync();
            }

            //Assert
            Assert.True(code == ResultCode.Ok);

            foreach (Sensor item in sensors!)
            {
                (code) = await supervisor.UpdateSensorAsync(item);
                if (code == ResultCode.Ok)
                {
                    break;
                }
            }

            Assert.True(code == ResultCode.Ok);
        }

        [Fact]
        public async Task DeleteSensor()
        {
            // Arrange
            await this.Initialyse();
            Sensor? sensor = new Sensor()
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
            };

            //Act
            ISupervisorSensor supervisor = this.HostApplication.Services.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor();
            ResultCode code = await supervisor.AddUpdateSensorAsync(sensor);
            if (code == ResultCode.Ok)
            {
                code = await supervisor.DeleteSensorAsync("1");
                if (code == ResultCode.Ok)
                {
                    (ResultCode code, IEnumerable<Sensor>? sensors) obj = await supervisor.GetSensorsAsync();
                    //Assert
                    Assert.True(obj.code == ResultCode.ItemNotFound);
                    Assert.False(obj.sensors!.Any());

                }
            }

            //Assert
            Assert.True(code == ResultCode.Ok);
        }
    }
}
