using System;
using Connect.Mobile.View.Controls;
using Connect.Mobile.ViewModel;
using Framework.Core.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Connect.Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationView : BasePage
    {
        const uint AnimationSpeed = 300;

        #region Property

        private NotificationViewModel ViewModel { get; }

        #endregion

        #region Constructor

        public NotificationView()
        {
            InitializeComponent();

            this.BindingContext = this.ViewModel = Host.Current.GetService<NotificationViewModel>();

            if (this.ViewModel.ExpandPopup == null)
                this.ViewModel.ExpandPopup = new Action<bool, bool, bool, bool>((bool lowerButton, bool upperButton, bool tempButton, bool humButton) => this.ExpandPopup(lowerButton, upperButton, tempButton, humButton));

            if (this.ViewModel.ClosePopuUp == null)
                this.ViewModel.ClosePopuUp = new Action(() => this.ClosePopup());
        }

        #endregion

        #region Method     

        protected override void OnAppearing()
        {
            base.OnAppearing();

            NotificationPopUpView.OnExpandTapped += ExpandPopup;
            NotificationPopUpView.OnCloseTapped += ClosePopup;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            NotificationPopUpView.OnExpandTapped -= ExpandPopup;
            NotificationPopUpView.OnCloseTapped -= ClosePopup;
        }

        private async void ExpandPopup(bool lowerButton, bool upperButton, bool tempButton, bool humButton)
        {
            PageFader.IsVisible = true;
            await PageFader.FadeTo(1, AnimationSpeed, Easing.SinInOut);

            double pageHeight = Height;
            double firstSection = NotificationPopUpView.FirstSectionHeight;

            this.NotificationPopUpView.Initialize(lowerButton, upperButton, tempButton, humButton);

            await this.NotificationPopUpView.TranslateTo(0, pageHeight - firstSection, AnimationSpeed, Easing.SinInOut);
        }

        private async void ClosePopup()
        {
            NotificationPopUpView.TranslateTo(0, Height, AnimationSpeed, Easing.SinInOut).FireAndForgetSafeAsync();
            await PageFader.FadeTo(0, AnimationSpeed, Easing.SinInOut);
            PageFader.IsVisible = false;
        }

        private void PageFader_Tapped(object sender, EventArgs e)
        {
            this.ClosePopup();
        }

        #endregion
    }
}