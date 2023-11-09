using AirZapto.Model.Healthcheck;
using Connect.Model.Healthcheck;

namespace Zapto.Web.Test.ViewModels
{
    public class HealthcheckViewModelUnitTest : BaseViewModelUnitTest
    {
        [Fact]
        public async Task GetModelTest1()
        {
            //Arrange
            HealthCheckConnect healthCheckConnect = new HealthCheckConnect()
            {
                Status = 0,
                Entries = new Connect.Model.Healthcheck.Entries()
                {
                    ErrorSystem = new Connect.Model.Healthcheck.ErrorSystem()
                    {
                        Data = new Connect.Model.Healthcheck.Data(),
                        Description = "Description",
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 1,
                        Tags = new List<string>() { "ErrorSystem" },
                    },
                    Memory = new Connect.Model.Healthcheck.Memory()
                    {
                        Data = new Connect.Model.Healthcheck.Data(),
                        Description = "Description",
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 2,
                        Tags = new List<string>() { "Memory" },
                    },
                    SensorsStatus = new Connect.Model.Healthcheck.SensorsStatus()
                    {
                        Data = new Connect.Model.Healthcheck.Data(),
                        Description = "Description",
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 3,
                        Tags = new List<string>() { "SensorsStatus" },
                    },
                    ServerIotConnection = new ServerIotConnection()
                    {
                        Description = "Description",
                        Data = new Connect.Model.Healthcheck.Data(),
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 4,
                        Tags = new List<string>() { "ServerIotConnection" },
                    },
                    SignalR = new SignalR()
                    {
                        Description = "Description",
                        Data = new Connect.Model.Healthcheck.Data(),
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 5,
                        Tags = new List<string>() { "SignalR" },
                    },
                    Sqlite = new Connect.Model.Healthcheck.Sqlite()
                    {
                        Description = "Description",
                        Data = new Connect.Model.Healthcheck.Data(),
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 6,
                        Tags = new List<string>() { "Sqlite" },
                    },
                },
            };

            HealthCheckAirZapto healthCheckAirZapto = new HealthCheckAirZapto()
            {
                Status = 0,
                Entries = new AirZapto.Model.Healthcheck.Entries()
                {
                    ErrorSystem = new AirZapto.Model.Healthcheck.ErrorSystem() 
                    { 
                        Data = new AirZapto.Model.Healthcheck.Data(),
                        Description = "Description",
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 1,
                        Tags = new List<string>() { "ErrorSystem" },
                    },
                    SensorsStatus = new AirZapto.Model.Healthcheck.SensorsStatus()
                    {
                        Data = new AirZapto.Model.Healthcheck.Data(),
                        Description = "Description",
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 1,
                        Tags = new List<string>() { "SensorsStatus" },                        
                    },
                    Memory = new AirZapto.Model.Healthcheck.Memory()
                    {
                        Data = new AirZapto.Model.Healthcheck.Data(),
                        Description = "Description",
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 2,
                        Tags = new List<string>() { "Memory" },
                    },
                    Sqlite = new AirZapto.Model.Healthcheck.Sqlite()
                    {
                        Description = "Description",
                        Data = new AirZapto.Model.Healthcheck.Data(),
                        Duration = "Duration",
                        Exception = new Exception(),
                        Status = 6,
                        Tags = new List<string>() { "Sqlite" },
                    },
                },
            };
            this.ApplicationHealthCheckConnectServices.Setup(x => x.GetHealthCheckConnect()).ReturnsAsync(healthCheckConnect);
            this.ApplicationHealthCheckAirZaptoServices.Setup(x => x.GetHealthCheckAirZapto()).ReturnsAsync(healthCheckAirZapto);

            //Act
            IHealthCheckViewModel vm = new HealthCheckViewModel(this.ServiceCollection.BuildServiceProvider());
            List<HealthCheckModel>? items = await vm.GetModelList();

            //Assert
            Assert.NotNull(items);
            Assert.Equal("Connect WebServer", items[0].Name);
            Assert.Equal("Air Zapto WebServer", items[1].Name);
        }
    }
}
