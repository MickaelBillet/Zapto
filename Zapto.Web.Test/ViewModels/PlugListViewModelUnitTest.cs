namespace Zapto.Web.Test.ViewModels
{
    public class PlugListViewModelUnitTest : BaseViewModelUnitTest
    {
        [Fact]
        public void GetPlugModels_Returns_PlugModels_List()
        {
            // Arrange
            var roomModel = new RoomModel
            {
                LocationId = "1",
                ConnectedObjectsList = new List<ConnectedObject>
                {
                    new ConnectedObject
                    {
                        Name = "Object1",
                        Plug = new Plug
                        {
                            ConditionType = 0,
                            Id = "1",
                            OnOff = 1,
                            Mode = Mode.Manual,
                            Order = 1,
                            Status = Status.ON,
                            WorkingDuration = 0.5f
                        }
                    },
                    new ConnectedObject
                    {
                        Name = "Object2",
                        Plug = null
                    },
                    new ConnectedObject
                    {
                        Name = "Object3",
                        Plug = new Plug
                        {
                            ConditionType = 1,
                            Id = "2",
                            OnOff = 0,
                            Mode = Mode.Manual,
                            Order = 2,
                            Status = Status.OFF,
                            WorkingDuration = 1.0f
                        }
                    }
                }
            };


            // Act
            IPlugListViewModel vm = new PlugListViewModel(this.ServiceCollection.BuildServiceProvider());
            var result = vm.GetPlugModels(roomModel);

            // Assert
            //Assert.NotNull(result);
            //Assert.IsType<List<PlugModel>>(result);
            //Assert.Equal(2, result.Count);
            //Assert.Equal("Object1", result[0].Name);
            //Assert.Equal("Object3", result[1].Name);
        }
    }
}
