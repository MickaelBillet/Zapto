using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Supervisors.Tests
{
    public class SupervisorCallTest : SupervisorBase
    {
        [Fact]
        public async Task AddCall()
        {
            //Arrange
            await this.Initialyse();

            //Act
            IDalSession session = this.HostApplication.Services.GetRequiredService<IDalSession>();
            IRepositoryFactory repositoryFactory = this.HostApplication.Services.GetRequiredService<IRepositoryFactory>();
            ISupervisorCall supervisor = new SupervisorCall(session, repositoryFactory);
            ResultCode code = await supervisor.AddCallOpenWeather();

            //Assert
            Assert.True(code == ResultCode.Ok);
        }

        [Fact]
        public async Task GetDayCallsCount()
        {
            //Arrange
            await this.Initialyse();

            //Act
            long? call = null;
            IDalSession session = this.HostApplication.Services.GetRequiredService<IDalSession>();
            IRepositoryFactory repositoryFactory = this.HostApplication.Services.GetRequiredService<IRepositoryFactory>();
            ISupervisorCall supervisor = new SupervisorCall(session, repositoryFactory);
            ResultCode code = await supervisor.AddCallOpenWeather();
            if (code == ResultCode.Ok)
            {
                call = await supervisor.GetDayCallsCount(Clock.Now);
            }

            //Assert
            Assert.True(code == ResultCode.Ok);
            Assert.True(call == 1);
        }
    }
}
