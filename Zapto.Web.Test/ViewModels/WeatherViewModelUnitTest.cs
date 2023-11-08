

using WeatherZapto.Model;

namespace Zapto.Web.Test.ViewModels
{
    public class WeatherViewModelUnitTest : BaseViewModelUnitTest
    {
        [Fact]
        public async Task GetWeatherModelTest1()
        {
            //Arrange
            this.LocalStorageService.Setup(x => x.GetItemAsync<string>(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("fr");
           
            var weather = new OpenWeather 
            { 
                main = new Main { temp = 280.0 }, 
                weather = new List<Weather> { new Weather { description = "Clear", icon = "01d" } }, 
                wind = new Wind { speed = 5.0, deg = 180 }, 
                name = "Boston",                
            };

            var claims = new OpenWeatherClaims { APIKey = "API_KEY",  BackendUrl = "Url"};
            var jsonClaims = JsonSerializer.Serialize(claims);

           //MB this.AuthenticationService.Setup(x => x.GetClaim(It.IsAny<string>())).ReturnsAsync(jsonClaims);
           //MB  this.ApplicationWeatherService.Setup(x => x.GetCurrentWeather(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(weather);
           //MB this.ApplicationWeatherService.Setup(x => x.GetCurrentWeatherImage(It.IsAny<string>())).ReturnsAsync(new MemoryStream());

            //Act
            WeatherViewModel vm = new WeatherViewModel(this.ServiceCollection.BuildServiceProvider());
          //  WeatherModel? model = await vm.GetWeatherModel(30, 60);

            //Assert
            //Assert.NotNull(model);
            //Assert.Equal("Clear", model.WeatherText);
            //Assert.Equal("Boston", model.Location);
            //Assert.Equal("6,9", model.Temperature);
            //Assert.Equal(180, model.WindDirection);
            //Assert.Equal("18", model.WindSpeed);
        }
    }
}
