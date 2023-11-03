using Connect.Application;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace Connect.Mobile.ViewModel
{
    public class MasterViewModel : BaseViewModel
	{
        private Location location = null;

        #region Properties

        public Location Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        #endregion

        #region Commands

        public ICommand SelectRoomCommand { get; }

        public ICommand ClickLocationCommand { get; }

		#endregion

		#region Services

        private IApplicationLocationServices ApplicationLocationServices { get; }

		#endregion

		#region Constructor

		public MasterViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
            this.SelectRoomCommand = new Command<Room>(async (Room room) => await ExecuteSelectRoomCommand(room));
            this.ClickLocationCommand = new Command(async () => await ExecuteClickLocationCommand());

            this.ApplicationLocationServices = serviceProvider.GetService<IApplicationLocationServices>();
        } 

        #endregion

        #region Method

		/// <summary>
		/// Initializes the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="item">Item.</param>
        public override void Initialize(Object item, Func<Object, Task> validateCallback = null, Func<Object, Task> cancelCallback = null)
		{
            this.LoadingTask = new NotifyTaskCompletion(Task.Run(() =>
            {
                if (item != null)
                {
                    this.Location = item as Location;

                    this.ApplicationLocationServices.GetLocation(this.location.Id).Subscribe<Location>
                    (
                        (location) =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                if (location != null)
                                {
                                    this.Location.RoomsList = location.RoomsList;
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

                    base.Initialize(item, validateCallback, cancelCallback);
                }
            }));
		}

        /// <summary>
        /// ExecuteClickLocationCommand
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteClickLocationCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
                await this.NavigationService.SetRootPage<DetailHomeViewModel>(this.Location);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, "ExecuteClickLocationCommand");
            }
            finally
			{
				IsBusy = false;
			}
		}

        /// <summary>
        /// ExecuteSelectRoomCommand
        /// </summary>
        /// <returns></returns>
		public async Task ExecuteSelectRoomCommand(Room room)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await this.NavigationService.NavigateToAsync<DetailViewModel>(room);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// On the disappearing.
        /// </summary>
        public override Task OnDisappearing()
        {
            return Task.FromResult<Boolean>(true);
        }

		#endregion
	}
}