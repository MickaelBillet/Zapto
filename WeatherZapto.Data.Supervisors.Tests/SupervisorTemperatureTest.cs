using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Data.Services.Repositories;
using WeatherZapto.Model;

namespace WeatherZapto.Data.Supervisors.Tests
{
    public class SupervisorTemperatureTest : SupervisorBase
    {
        [Fact]
        public async Task GetTemperatures()
        {
            //Arrange
            this.CreateHost();
            await this.Initialyse();
            ResultCode code = await this.AddWeatherData();

            //Act
            ISupervisorTemperature supervisor = new SupervisorTemperature(this.HostApplication!.Services);
            IEnumerable<double> tempsHouilles = await supervisor.GetTemperatures("Houilles", Clock.Now.ToUniversalTime());
            IEnumerable<double> tempsMarseille = await supervisor.GetTemperatures("Marseille", Clock.Now.ToUniversalTime());

            //Assert
            Assert.Equal(new List<double>() { 10, 20 }, tempsHouilles);
            Assert.Equal(new List<double>() { 25, 35 }, tempsMarseille);
        }

        [Fact]
        public async Task GetTemperatureMin()
        {
            //Arrange
            this.CreateHost();
            await this.Initialyse();
            ResultCode code = await this.AddWeatherData();

            //Act
            ISupervisorTemperature supervisor = new SupervisorTemperature(this.HostApplication!.Services);
            double? tempHouilles =  await supervisor.GetTemperatureMin("Houilles", Clock.Now.ToUniversalTime());
            double? tempMarseille = await supervisor.GetTemperatureMin("Marseille", Clock.Now.ToUniversalTime());

            //Assert
            Assert.Equal(10, tempHouilles);
            Assert.Equal(25, tempMarseille);
        }

        [Fact]
        public async Task GetTemperatureMax()
        {
            //Arrange
            this.CreateHost();
            await this.Initialyse();
            ResultCode code = await this.AddWeatherData();

            //Act
            ISupervisorTemperature supervisor = new SupervisorTemperature(this.HostApplication!.Services);
            double? tempHouilles = await supervisor.GetTemperatureMax("Houilles", Clock.Now.ToUniversalTime());
            double? tempMarseille = await supervisor.GetTemperatureMax("Marseille", Clock.Now.ToUniversalTime());

            //Assert
            Assert.Equal(20, tempHouilles);
            Assert.Equal(35, tempMarseille);
        }

        private async Task<ResultCode> AddWeatherData()
        {
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

            if (code == ResultCode.Ok)
            {
                code = await supervisor.AddWeatherAsync(new ZaptoWeather()
                {
                    Date = Clock.Now,
                    WindDirection = 100,
                    FeelsLike = "100",
                    Latitude = 10d,
                    Longitude = 20d,
                    Location = "Houilles",
                    Temperature = "20",
                    TemperatureMax = "25",
                    TemperatureMin = "15",
                    Pressure = 200,
                    WeatherText = "toto",
                    TimeSpam = Clock.Now.ToUniversalTime(),
                    WindSpeed = "300"
                });

            }

            if (code == ResultCode.Ok)
            {
                code = await supervisor.AddWeatherAsync(new ZaptoWeather()
                {
                    Date = Clock.Now,
                    WindDirection = 100,
                    FeelsLike = "100",
                    Latitude = 15d,
                    Longitude = 25d,
                    Location = "Marseille",
                    Temperature = "25",
                    TemperatureMax = "25",
                    TemperatureMin = "15",
                    Pressure = 200,
                    WeatherText = "toto",
                    TimeSpam = Clock.Now.ToUniversalTime(),
                    WindSpeed = "300"
                });

            }

            if (code == ResultCode.Ok)
            {
                code = await supervisor.AddWeatherAsync(new ZaptoWeather()
                {
                    Date = Clock.Now,
                    WindDirection = 100,
                    FeelsLike = "100",
                    Latitude = 15d,
                    Longitude = 25d,
                    Location = "Marseille",
                    Temperature = "35",
                    TemperatureMax = "25",
                    TemperatureMin = "15",
                    Pressure = 200,
                    WeatherText = "toto",
                    TimeSpam = Clock.Now.ToUniversalTime(),
                    WindSpeed = "300"
                });

            }

            return code;
        }
    }
}
