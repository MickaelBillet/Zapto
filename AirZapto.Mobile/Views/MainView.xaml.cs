using System;
using AirZapto.Mobile;
using AirZapto.ViewModel;
using Framework.Core.Base;
using Xamarin.Forms;

namespace AirZapto.View
{
	public partial class MainView : ContentPage
	{
		const uint AnimationSpeed = 300;

		#region Property

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(MainViewModel), typeof(MainView), null);

		public MainViewModel ViewModel
		{
			get { return (MainViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		#endregion

		#region Constructor

		public MainView()
		{
			InitializeComponent();

			this.BindingContext = this.ViewModel = Host.Current.GetService<MainViewModel>();

			if (this.ViewModel.ClosePopuUp == null)
				this.ViewModel.ClosePopuUp = new Action(() => this.ClosePopup());
		}

		#endregion

		#region Methods

		private void TapGestureThemeTapped(object sender, EventArgs e)
		{
			App.Current.UserAppTheme = (App.Current.UserAppTheme == OSAppTheme.Light)
				? OSAppTheme.Dark
				: OSAppTheme.Light;
		}

		private async void ExpandPopup()
		{
			PageFader.IsVisible = true;
			await PageFader.FadeTo(1, AnimationSpeed, Easing.SinInOut);

			double pageHeight = Height;
			double firstSection = this.MenuPopupView.FirstSectionHeight;

			await this.MenuPopupView.TranslateTo(0, pageHeight - firstSection, AnimationSpeed, Easing.SinInOut);
		}

		private async void ClosePopup()
		{
			this.MenuPopupView.TranslateTo(0, Height, AnimationSpeed, Easing.SinInOut).FireAndForgetSafeAsync();
			await PageFader.FadeTo(0, AnimationSpeed, Easing.SinInOut);
			PageFader.IsVisible = false;
		}

		private void PageFader_Tapped(object sender, EventArgs e)
		{
			this.ClosePopup();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		private void TapGestureMenuTapped(object sender, EventArgs e)
		{
			this.ExpandPopup();
		}

		#endregion
	}
}
