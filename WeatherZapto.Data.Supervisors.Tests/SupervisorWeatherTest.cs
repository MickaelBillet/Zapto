using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Data.Services.Repositories;
using WeatherZapto.Model;

namespace WeatherZapto.Data.Supervisors.Tests
{
    public class SupervisorWeatherTest : SupervisorBase
    {
        [Fact]
        public async Task AddWeather()
        {
            //Arrange
            this.CreateHost();
            await this.Initialyse();

            //Act
            IDalSession session = this.HostApplication!.Services.GetRequiredService<IDalSession>();
            IRepositoryFactory repositoryFactory = this.HostApplication.Services.GetRequiredService<IRepositoryFactory>();
            ISupervisorWeather supervisor = new SupervisorWeather(session, repositoryFactory);
            ResultCode code = await supervisor.AddWeatherAsync(new ZaptoWeather()
            {
                Date = Clock.Now,
                WindDirection = 100,
                FeelsLike = "100",
                Latitude = 10d,
                Longitude = 20d,
                Location = "Houilles",
                Temperature = "10",
                TemperatureMax = "15",
                TemperatureMin = "5",
                Pressure = 200,
                WeatherText = "toto",
                TimeSpam = Clock.Now.ToUniversalTime(),
                WindSpeed = "300"
            });

            //Assert
            Assert.True(code == ResultCode.Ok);
        }
    }
}
