namespace Zapto.Web.Test.ViewModels
{
    public class LocationViewModelUnitTest : BaseViewModelUnitTest
    {
        [Fact]
        public async Task GetLocationModel_ReturnsLocationModel()
        {
            // Arrange
            var mockLocation = new Location { Id = "1", City = "New York" };
            this.ApplicationLocationServices.Setup(s => s.GetLocations()).ReturnsAsync(new List<Location> { mockLocation });

            var viewModel = new LocationViewModel(this.ServiceCollection.BuildServiceProvider());

            // Act
            var result = await viewModel.GetConnectLocationModel();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<LocationModel>(result);
            Assert.Equal(mockLocation.Id, result.Id);
            Assert.Equal(mockLocation.City, result.Name);
        }
    }
}
