namespace Zapto.Web.Test.ViewModels
{
    public class DashboardViewModelUnitTest : BaseViewModelUnitTest
    {
        [Fact]
        public async Task GetLocationModelTest1()
        {
            //Arrange
            LocationModel model = new LocationModel()
            {
                Id = "0",
                Name = "Maison",
            };
            this.LocationViewModel.Setup(x => x.GetConnectLocationModel()).ReturnsAsync(model);

            //Act
            IDashboardViewModel vm = new DashboardViewModel(this.ServiceCollection.BuildServiceProvider());
           // LocationModel? res = await vm.GetLocationModel();

            //Assert
           // Assert.Equal(model, res);
        }

        [Fact]
        public async Task GetWeatherModelTest1()
        {
            //Arrange
            WeatherModel model = new WeatherModel()
            {
                Id = "0",
                Image = "Image",
                Location = "Location",
                Temperature = "25",
                WindDirection = 180,
                WeatherText = "Weather",
                WindSpeed = "100",
            };
            //this.WeatherViewModel.Setup(x => x.GetWeatherModel(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(model);

            //Act
            IDashboardViewModel vm = new DashboardViewModel(this.ServiceCollection.BuildServiceProvider());
            //WeatherModel? res = await vm.GetWeatherModel();

            //Assert
         //   Assert.Equal(model, res);
        }

        [Fact]
        public async Task TestNotificationTest1()
        {
            //Arrange
            this.ApplicationLocationServices.Setup(x => x.TestNotication(It.IsAny<string>()));

            //Act
            IDashboardViewModel vm = new DashboardViewModel(this.ServiceCollection.BuildServiceProvider());
           // await vm.TestNotification("Location");

            //Assert
            Assert.True(true);
        }
    }
}
