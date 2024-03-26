using AirZapto.Model;
using Framework.Core.Base;

namespace AirZapto.Data.Supervisors.Tests
{
    public class SupervisorSensorTest : SupervisorBase
    {
        public SupervisorSensorTest() : base() 
        { 
        }


        [Fact]
        public async Task Test1()
        {
            // Arrange
            await this.Initialyse();

            //Act
            ISupervisorSensor supervisor = new SupervisorSensor(this.HostApplication.Services);
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
            Assert.True(code == ResultCode.ArgumentRequired);
        }
    }
}
