namespace Zapto.Web.Test.ViewModels
{
    public class SensorEventListViewModelUnitTest : BaseViewModelUnitTest
    {
        [Fact]
        public void GetSensorEventModels_ReturnsCorrectSensorEventModels()
        {
            // Arrange
            var roomModel = new RoomModel()
            {
                LocationId = "locationId",
                Sensors = new List<Sensor>()
                {
                    new Sensor()
                    {
                        Name = "Sensor 1",
                        Channel = "1",
                        Type = DeviceType.Sensor_Water_Leak,
                        LeakDetected = 0,
                        Id = "1"
                    },
                    new Sensor()
                    {
                        Name = "Sensor 2",
                        Channel = "2",
                        Type = DeviceType.Sensor_Water_Leak,
                        LeakDetected = 1,
                        Id = "2"
                    }
                }
            };

            var expectedSensorEventModels = new List<SensorEventModel>()
            {
                new SensorEventModel()
                {
                    Name = "Sensor 1",
                    LocationId = "locationId",
                    Id = "1",
                    HasLeak = 0
                },
                new SensorEventModel()
                {
                    Name = "Sensor 2",
                    LocationId = "locationId",
                    Id = "2",
                    HasLeak = 1
                }
            };

            // Act
            var viewModel = new SensorEventListViewModel(this.ServiceCollection.BuildServiceProvider());
            var actualSensorEventModels = viewModel.GetSensorEventModels(roomModel);

            // Assert
            Assert.NotNull(actualSensorEventModels);
            Assert.Equal(expectedSensorEventModels.Count, actualSensorEventModels.Count());

            foreach (var expectedSensorEventModel in expectedSensorEventModels)
            {
                var actualSensorEventModel = actualSensorEventModels.FirstOrDefault(x => x.Id == expectedSensorEventModel.Id);
                Assert.NotNull(actualSensorEventModel);
                Assert.Equal(expectedSensorEventModel.Name, actualSensorEventModel.Name);
                Assert.Equal(expectedSensorEventModel.LocationId, actualSensorEventModel.LocationId);
                Assert.Equal(expectedSensorEventModel.HasLeak, actualSensorEventModel.HasLeak);
            }
        }
    }
}
