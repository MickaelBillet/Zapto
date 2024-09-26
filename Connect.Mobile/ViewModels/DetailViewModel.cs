using Connect.Application;
using Connect.Application.Infrastructure;
using Connect.Mobile.Resources;
using Connect.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace Connect.Mobile.ViewModel
{
    public class DetailViewModel : BaseViewModel
    {
        private Room room = null;

        #region Properties

        public Room Room
        {
            get { return room; }
            set { SetProperty(ref room, value); }
        }

        public ICommand NotificationCommand
        {
            get; set;
        }

		#endregion

		#region Services

        private IApplicationRoomServices ApplicationRoomServices { get; }
        private IApplicationNotificationServices ApplicationNotificationServices { get; }
        private ISignalRService SignalRService { get; }
        private ICacheService CacheService { get; }

        #endregion

        #region Constructor

        public DetailViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.NotificationCommand = new Command(async () => await ExecuteNotificationsCommand());

            this.ApplicationRoomServices = serviceProvider.GetService<IApplicationRoomServices>();
            this.ApplicationNotificationServices = serviceProvider.GetService<IApplicationNotificationServices>();
            this.SignalRService = serviceProvider.GetService<ISignalRService>();
            this.CacheService = serviceProvider.GetService<ICacheService>();
        }

        #endregion

        #region Methods
        public override void Initialize(object item, 
                                        Func<object, Task> validateCallback = null, 
                                        Func<object, Task> cancelCallback = null)
        {
            try
            {
                if (item != null)
                {
                    this.Room = item as Room;
                    this.RefreshData();
                    base.Initialize(item, validateCallback, cancelCallback);

                    this.SignalRService.StartAsync(
                        App.LocationId,
                        async (plugStatus) =>
                        {
                            ConnectedObject obj = this.Room.ConnectedObjectsList.FirstOrDefault<ConnectedObject>((ConnectedObject arg) => arg.Plug?.Id == plugStatus.PlugId);
                            if (obj?.Plug != null)
                            {
                                obj.Plug.WorkingDuration = plugStatus.WorkingDuration;
                                obj.Plug.Status = plugStatus.Status;
                                obj.Plug.Order = plugStatus.Order;
                                obj.Plug.Mode = plugStatus.Mode;
                                obj.Plug.OnOff = plugStatus.OnOff;
                            }
                            await this.CacheService.InsertObject<Room>("room" + this.Room.Id, this.Room);
                        },
                        async (roomStatus) =>
                        {
                            if (this.Room.Id == roomStatus.RoomId)
                            {
                                this.Room.Temperature = roomStatus.Temperature;
                                this.Room.Humidity = roomStatus.Humidity;
                                this.Room.Pressure = roomStatus.Pressure;
                                await this.CacheService.InsertObject<Room>("room" + this.Room.Id, this.Room);
                            }
                        },
                        async (sensorStatus) =>
                        {
                            ConnectedObject obj = this.Room.ConnectedObjectsList.FirstOrDefault<ConnectedObject>((ConnectedObject arg) => arg.Sensor?.Id == sensorStatus.SensorId);
                            if (obj?.Sensor != null)
                            {
                                obj.Sensor.Temperature = sensorStatus.Temperature;
                                obj.Sensor.Pressure = sensorStatus.Pressure;
                                obj.Sensor.Humidity = sensorStatus.Humidity;
                                obj.Sensor.IsRunning = (byte)sensorStatus.IsRunning;
                            }
                            await this.CacheService.InsertObject<Room>("room" + this.Room.Id, this.Room);
                        },
                        null).FireAndForgetSafeAsync();
                }
            }
            finally
			{
               IsInitialized = true;
			}
        }

        public async Task ExecuteNotificationsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await this.NavigationService.NavigateToModalAsync<NotificationViewModel>(this.Room, async (obj) => await this.ValidateCbNotification(obj), null);

                this.HandleError(Model.ErrorType.None, string.Empty);
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

        private async Task ValidateCbNotification(object item)
        {
            try
            {
                if (this.IsConnected)
                {
                    Notification notification = (item as Notification).Clone<Notification>();

                    if (await this.ApplicationNotificationServices.AddUpdateNotificationAsync(this.Room, notification) == false)
                    {
                        this.HandleError(Model.ErrorType.ErrorSoftware, AppResources.ErrorNotification);
                    }
                    else
                    {
                        this.HandleError(Model.ErrorType.None, String.Empty);
                    }
                }
                else
                {
                    this.HandleError(Model.ErrorType.Warning, AppResources.NotConnected);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                this.HandleError(Model.ErrorType.ErrorWebService, ex.Message);
            }
            finally
            {
                await Task.FromResult(true);
            }
        }

        protected override void RefreshData()
        {
            this.LoadingTask = new NotifyTaskCompletion(Task.Run(() =>
            {
                this.ApplicationRoomServices.GetRoom(this.Room.Id).Subscribe<Room>
                ((room) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (room != null)
                        {
                            this.Room.Temperature = room.Temperature;
                            this.Room.Humidity = room.Humidity;
                            this.Room.Pressure = room.Pressure;
                            this.Room.ConnectedObjectsList = room.ConnectedObjectsList.OrderBy((arg) => (arg.DeviceType)).ToList<ConnectedObject>();
                            this.Room.NotificationsList = room.NotificationsList;
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
                });
            }));
        }

        #endregion
    }
}
