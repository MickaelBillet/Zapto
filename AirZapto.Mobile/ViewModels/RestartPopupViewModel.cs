using AirZapto.Application;
using AirZapto.Mobile.Resources;
using AirZapto.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace AirZapto.ViewModel
{
    public class RestartPopupViewModel : BaseViewModel
	{
		private Sensor _sensor = null;

		#region Properties

		public Sensor Sensor
		{
			get { return _sensor; }

			set
			{
				SetProperty<Sensor>(ref _sensor, value);
				Debug.WriteLine(_sensor.Description);
			}
		}
		#endregion

		#region Command

		public ICommand RestartCommand { get; set; }

		public ICommand CloseCommand { get; set; }

		#endregion

		#region Services

		private IApplicationSensorServices ApplicationSensorServices { get; }

		#endregion

		#region Constructor

		public RestartPopupViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.RestartCommand = new Command<object>(async (obj) => await ExecuteRestartCommand(obj), (obj) => CanRestartCommand(obj));
			this.CloseCommand = new Command(async () => await ExecuteCloseCommand());

			this.ApplicationSensorServices = serviceProvider.GetService<IApplicationSensorServices>();
		}

		#endregion

		#region Methods

		public override async Task Initialize(Object item, Func<Object, Task> validateCallback = null, Func<Object, Task> cancelCallback = null)
		{
			try
			{
				this.Sensor = item as Sensor;

				await base.Initialize(null, validateCallback, cancelCallback);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				this.ErrorHandlerService.TriggerError(new Error(ex.Message, null, true, false), this.DisplayErrorDialogBox);
			}
			finally
			{
			}
		}

		public bool CanRestartCommand(object parameter)
		{
			return true;
		}

		public async Task ExecuteRestartCommand(object parameter)
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				await this.NavigationService.RemovePopupAsync();

				if (await this.ApplicationSensorServices.RestartAsync(this.Sensor) != true)
				{
					this.ErrorHandlerService.TriggerError(new Error(AppResources.ErrorRestart, Errors.ERR0005_ErrorRestart, true, true), this.DisplayErrorDialogBox);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				this.ErrorHandlerService.TriggerError(new Error(ex.Message, null, true, false), this.DisplayErrorDialogBox);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public async Task ExecuteCloseCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				await this.NavigationService.RemovePopupAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				this.ErrorHandlerService.TriggerError(new Error(ex.Message, null, true, false), this.DisplayErrorDialogBox);
			}
			finally
			{
				IsBusy = false;
			}
		}

		#endregion
	}
}
