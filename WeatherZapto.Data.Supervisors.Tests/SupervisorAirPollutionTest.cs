using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Data.Services.Repositories;
using WeatherZapto.Model;

namespace WeatherZapto.Data.Supervisors.Tests
{
    public class SupervisorAirPollutionTest : SupervisorBase
    {
        [Fact]
        public async Task AddAirPollution()
        {
            //Arrange
            this.CreateHost();
            await this.Initialyse();

            //Act
            IDalSession session = this.HostApplication!.Services.GetRequiredService<IDalSession>();
            IRepositoryFactory repositoryFactory = this.HostApplication.Services.GetRequiredService<IRepositoryFactory>();
            ISupervisorAirPollution supervisor = new SupervisorAirPollution(session, repositoryFactory);
            ResultCode code = await supervisor.AddAirPollutionAsync(new ZaptoAirPollution()
            {
                aqi = 5d,
                co = 100,
                Date = Clock.Now,
                Latitude = 10d,
                Longitude = 20d,
                so2 = 200,
                Location = "Houilles",
                nh3 = 300,
                no = 400,
                TimeStamp = Clock.Now.ToUniversalTime(),
                no2 = 500,
                o3 = 600,
                pm10 = 700,
                pm2_5 = 800
            });

            //Assert
            Assert.True(code == ResultCode.Ok);
        }
    }
}
