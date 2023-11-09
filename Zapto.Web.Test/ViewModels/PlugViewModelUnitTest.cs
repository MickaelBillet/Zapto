namespace Zapto.Web.Test.ViewModels
{
    public class PlugViewModelUnitTest : BaseViewModelUnitTest
    {
        [Fact]
        public async Task ReceiveStatusAsync_ShouldUpdateModel_WhenSignalRServiceReceivesUpdate()
        {
            //Arrange
            var model = new PlugModel
            {
                Id = "plugId",
                LocationId = "locationId",
                Status = 3,
                WorkingDuration = 200,
                Command = 2
            };

            var receivedPlugStatus = new PlugStatus
            {
                PlugId = "plugId",
                Status = Status.ON,
                Order = 0,
                WorkingDuration = 200,
                OnOff = 1,
                Mode = Mode.Manual
            };

            this.SignalRService.Setup(s => s.StartAsync(It.IsAny<string>(), It.IsAny<Action<PlugStatus>>(), It.IsAny<Action<RoomStatus>>(), It.IsAny<Action<SensorStatus>>(), It.IsAny<Action<NotificationStatus>>()))
                                .Callback<string, Action<PlugStatus>, Action<RoomStatus>, Action<SensorStatus>, Action<NotificationStatus>>((locationId, onReceive, onStart, onReconnect, onDisconnect) =>
                                {
                                    onReceive.Invoke(receivedPlugStatus);
                                })
                                .ReturnsAsync(true);

            //Act
            IPlugViewModel viewModel = new PlugViewModel(this.ServiceCollection.BuildServiceProvider());
            var result = await viewModel.ReceiveStatusAsync(model);

            //Assert
            viewModel.Refresh += (sender, args) => Assert.True(true); // Verify OnRefresh event is invoke
            Assert.True(result);
            Assert.Equal("plugId", model.Id);
            Assert.Equal("locationId", model.LocationId);
            Assert.Equal(3, model.Status);
            Assert.Equal(200, model.WorkingDuration);
            Assert.Equal(2, model.Command);
        }

        [Fact]
        public async Task SendCommandAsync_CommandIsOne_ShouldCallChangeModeWithOffManual()
        {
            // Arrange
            var command = 1;
            var id = "plugId";

            // Act
            IPlugViewModel vm = new PlugViewModel(this.ServiceCollection.BuildServiceProvider());
            await vm.SendCommandAsync(command, id);

            // Assert
            this.ApplicationPlugServices.Verify(
                x => x.ChangeMode(
                    It.Is<Plug>(p =>
                        p.Id == id &&
                        p.OnOff == Status.OFF &&
                        p.Mode == Mode.Manual
                    )
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task SendCommandAsync_CommandIsTwo_ShouldCallChangeModeWithOnManual()
        {
            // Arrange
            var command = 2;
            var id = "plugId";

            // Act
            IPlugViewModel vm = new PlugViewModel(this.ServiceCollection.BuildServiceProvider());
            await vm.SendCommandAsync(command, id);

            // Assert
            this.ApplicationPlugServices.Verify(
                x => x.ChangeMode(
                    It.Is<Plug>(p =>
                        p.Id == id &&
                        p.OnOff == Status.ON &&
                        p.Mode == Mode.Manual
                    )
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task SendCommandAsync_CommandIsThree_ShouldCallChangeModeWithOnPrograming()
        {
            // Arrange
            var command = 3;
            var id = "plugId";

            // Act
            IPlugViewModel vm = new PlugViewModel(this.ServiceCollection.BuildServiceProvider());
            await vm.SendCommandAsync(command, id);

            // Assert
            this.ApplicationPlugServices.Verify(
                x => x.ChangeMode(
                    It.Is<Plug>(p =>
                        p.Id == id &&
                        p.OnOff == Status.ON &&
                        p.Mode == Mode.Programing
                    )
                ),
                Times.Once
            );
        }
    }
}
