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
    public class CalibrationPopupViewModel : BaseViewModel
	{
		#region Properties

		private Sensor Sensor { get; set; }

		#endregion

		#region Command

		public ICommand CalibrateCommand { get; set; }

		public ICommand CloseCommand { get; set; }

		#endregion

		#region Services

		private IApplicationSensorServices ApplicationSensorServices { get; }

		#endregion

		#region Constructor

		public CalibrationPopupViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.CalibrateCommand = new Command<object>(async (obj) => await ExecuteCalibrateCommand(obj), (obj) => CanCalibrateCommand(obj));
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

		public bool CanCalibrateCommand(object parameter)
		{
			return true;
		}

		public async Task ExecuteCalibrateCommand(object parameter)
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				await this.NavigationService.RemovePopupAsync();

				if (await this.ApplicationSensorServices.CalibrationAsync(this.Sensor) != true)
				{
					this.ErrorHandlerService.TriggerError(new Error(AppResources.ErrorCalibration, Errors.ERR0004_ErrorCalibration, true, true), this.DisplayErrorDialogBox);
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
