using System;
using Connect.Mobile.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Connect.Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPopUpView : ContentView
    {
        public delegate void ClickExpandDelegate(bool lowerButton, bool upperButton, bool tempButton, bool humButton);

        public delegate void ClickCloseDelegate();

        public ClickExpandDelegate OnExpandTapped { get; set; }

        public ClickCloseDelegate OnCloseTapped { get; set; }

        #region Property

        #endregion

        #region Constructor

        public NotificationPopUpView()
        {
            InitializeComponent();
        }

        #endregion

        #region Method     

        public void Initialize(bool lowerButton, bool upperButton, bool tempButton, bool humButton)
        {
            this.LowerButton.IsToggled = lowerButton;
            this.UpperButton.IsToggled = upperButton;
            this.TemperatureButton.IsToggled = tempButton;
            this.HumidityButton.IsToggled = humButton;

            this.UpdateButtonStatus();

            this.TitleLabel.Text = ((this.TemperatureButton.IsToggled || this.HumidityButton.IsToggled)
                                            && (this.LowerButton.IsToggled || this.UpperButton.IsToggled)) ? AppResources.EditNotification : AppResources.AddNotification;
        }

        public double FirstSectionHeight
        {
            get
            {
                return FirstSection.Height;
            }
        }

        private void TemperatureButton_Clicked(object sender, EventArgs e)
        {
            this.HumidityButton.IsToggled = !this.TemperatureButton.IsToggled;

            this.UpdateButtonStatus();
        }

        private void HumidityButton_Clicked(object sender, EventArgs e)
        {
            this.TemperatureButton.IsToggled = !this.HumidityButton.IsToggled;

            this.UpdateButtonStatus();
        }

        private void LowerButton_Clicked(object sender, EventArgs e)
        {
            this.UpperButton.IsToggled = !this.LowerButton.IsToggled;

            this.UpdateButtonStatus();
        }

        private void UpperButton_Clicked(object sender, EventArgs e)
        {
            this.LowerButton.IsToggled = !this.UpperButton.IsToggled;

            this.UpdateButtonStatus();
        }

        private void UpdateButtonStatus()
        {
            this.OkButton.IsEnabled = ((this.TemperatureButton.IsToggled || this.HumidityButton.IsToggled)
                                            && (this.LowerButton.IsToggled || this.UpperButton.IsToggled)) ? true : false;
        }

        #endregion
    }
}