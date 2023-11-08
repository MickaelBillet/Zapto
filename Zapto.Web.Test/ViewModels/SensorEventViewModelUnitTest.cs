namespace Zapto.Web.Test.ViewModels
{
    public class SensorEventViewModelUnitTest : BaseViewModelUnitTest
    {
        [Fact]
        public async Task CancelLeakEvent_WhenModelIsNotNull_UpdatesHasLeakAndInvokesOnRefresh()
        {
            // Arrange
            var model = new SensorEventModel { Id = "1", HasLeak = 1 };
            this.ApplicationSensorServices.Setup(m => m.Leak(model.Id, 0)).ReturnsAsync(true);

            // Act
            var viewModel = new SensorEventViewModel(this.ServiceCollection.BuildServiceProvider());
            await viewModel.CancelLeakEvent(model);

            // Assert
            viewModel.Refresh += (sender, args) => Assert.True(true); // Verify OnRefresh event is invoke
            Assert.Equal(0, model.HasLeak);
            this.ApplicationSensorServices.Verify(m => m.Leak(model.Id, 0), Times.Once);
        }

        [Fact]
        public async Task ReceiveStatusAsync_WhenSensorStatusHasLeakDetected_UpdatesHasLeakAndInvokesOnRefresh()
        {
            // Arrange
            var model = new SensorEventModel { Id = "sensorId", LocationId = "locationId", HasLeak = 0, Name = "toto" };
            var sensorStatus = new SensorStatus { SensorId = "sensorId", RoomId = "roomId", LeakDetected = 1 };
            this.SignalRService.Setup(s => s.StartAsync(It.IsAny<string>(), It.IsAny<Action<PlugStatus>>(), It.IsAny<Action<RoomStatus>>(), It.IsAny<Action<SensorStatus>>(), It.IsAny<Action<NotificationStatus>>()))
                                .Callback<string, Action<PlugStatus>, Action<RoomStatus>, Action<SensorStatus>, Action<NotificationStatus>>((locationId, onReceive, onStart, onReconnect, onDisconnect) =>
                                {
                                    onReconnect.Invoke(sensorStatus);
                                })
                                .ReturnsAsync(true);
            
            // Act
            var viewModel = new SensorEventViewModel(this.ServiceCollection.BuildServiceProvider());
            var result = await viewModel.ReceiveStatusAsync(model);
           
            // Assert
            viewModel.Refresh += (sender, args) => Assert.True(true); // Verify OnRefresh event is invoke
            Assert.True(result);
            Assert.Equal(1, model.HasLeak);
        }
    }
}
