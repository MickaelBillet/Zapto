using Connect.Application;
using Connect.Application.Infrastructure;
using Connect.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Connect.Mobile.ViewModel
{
    public class DetailHomeViewModel : BaseViewModel
    {
        private ObservableCollection<Room> roomsList = null;

        #region Properties

        public Location Location
        {
            get; set;
        }

        public ObservableCollection<Room> RoomsList
        {
            get { return roomsList; }
            set { SetProperty(ref roomsList, value); }
        }

        public ICommand SelectRoomCommand
        {
            get; set;
        }

		#endregion

		#region Services

        private IApplicationConnectLocationServices ApplicationLocationServices { get; }
        private ISignalRService SignalRService { get; }
        private ICacheService CacheService { get; }

        #endregion

        #region Constructor

        public DetailHomeViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.RoomsList = new ObservableCollection<Room>();
            this.SelectRoomCommand = new Command<Room>(async (Room room) => await ExecuteSelectRoomCommand(room));

            this.ApplicationLocationServices = serviceProvider.GetService<IApplicationConnectLocationServices>();
            this.SignalRService = serviceProvider.GetService<ISignalRService>();
            this.CacheService = serviceProvider.GetService<ICacheService>();
        }

        #endregion

        #region Method

        public override void Initialize(Object item, Func<Object, Task> validateCallback = null, Func<Object, Task> cancelCallback = null)
        {
            try
            {
                if (item != null)
                {
                    this.Location = item as Location;

                    this.RefreshData();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.Title = this.Location.City;
                    });

                    this.SignalRService.StartAsync(
                        App.LocationId,
                        null,
                        (roomStatus) =>
                        {
                            Room room = this.Location.RoomsList.FirstOrDefault<Room>((Room arg) => arg.Id == roomStatus.RoomId);
                            if (room != null)
                            {
                                room.Temperature = roomStatus.Temperature;
                                room.Humidity = roomStatus.Humidity;
                                room.Pressure = roomStatus.Pressure;
                                this.CacheService.InsertObject<Room>("room" + room.Id, room);
                            }
                        },
                        null,
                        null);

                    base.Initialize(item, validateCallback, cancelCallback);
                }
            }
            finally
			{
                IsInitialized = true;
			}
        }

        public async Task ExecuteSelectRoomCommand(Room room)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await this.NavigationService.NavigateToAsync<DetailViewModel>(room);

                this.HandleError(Model.ErrorType.None, String.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                this.HandleError(Model.ErrorType.ErrorWebService, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override void RefreshData()
        {
            this.LoadingTask = new NotifyTaskCompletion(Task.Run(() =>
            {
                this.ApplicationLocationServices.GetLocation(this.Location.Id).Subscribe<Location>
                (
                    (location) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (location != null)
                            {
                                this.RoomsList.Clear();

                                foreach (Room room in location.RoomsList)
                                {
                                    if (((room.DeviceType & DeviceType.Sensor_Humidity) == DeviceType.Sensor_Humidity) 
                                        || ((room.DeviceType & DeviceType.Sensor_Pressure) == DeviceType.Sensor_Pressure)
                                        || ((room.DeviceType & DeviceType.Sensor_Temperature) == DeviceType.Sensor_Temperature))
                                    {
                                        this.RoomsList.Add(room);
                                    }
                                }
                            }
                        });
                    },
                    (Exception exception) =>
                    {
                        this.HandleError(Model.ErrorType.ErrorWebService, exception.Message);
                    },
                    () =>
                    {
                        this.HandleError(Model.ErrorType.None, string.Empty);
                    }
                );
            }));
        }

        #endregion
    }
}
