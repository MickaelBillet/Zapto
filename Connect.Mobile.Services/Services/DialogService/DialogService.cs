using System;
using System.Threading.Tasks;

namespace Connect.Mobile.Services
{
    public class DialogService : IDialogService
	{
		public async Task ShowError(string message,
			string title,
			string buttonText,
			Action afterHideCallback)
		{
			await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
				title,
				message,
				buttonText);

			if (afterHideCallback != null)
			{
				afterHideCallback();
			}
		}

		public async Task ShowError(
			Exception error,
			string title,
			string buttonText,
			Action afterHideCallback)
		{
			await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
				title,
				error.Message,
				buttonText);

			if (afterHideCallback != null)
			{
				afterHideCallback();
			}
		}

		public async Task ShowMessage(
			string message,
			string title)
		{
			await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
				title,
				message,
                AppResources.OK);
		}

		public async Task ShowMessage(
			string message,
			string title,
			string buttonText,
			Action afterHideCallback)
		{
			await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
				title,
				message,
				buttonText);

			if (afterHideCallback != null)
			{
				afterHideCallback();
			}
		}

		public async Task<bool> ShowMessage(
			string message,
			string title,
			string buttonConfirmText,
			string buttonCancelText,
			Action<bool> afterHideCallback)
		{
			bool result = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
				title,
				message,
				buttonConfirmText,
				buttonCancelText);

			if (afterHideCallback != null)
			{
				afterHideCallback(result);
			}
			return result;
		}

		public async Task ShowMessageBox(
			string message,
			string title)
		{
			await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
				title,
				message,
                AppResources.OK);
		}
	}
}
