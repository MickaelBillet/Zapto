using Connect.Mobile.ViewModel;
using Framework.Core.Domain;
using System;
using Xamarin.Forms;

namespace Connect.Mobile.View
{
    public partial class SensorCellView : ContentView
    {
        #region Property

        public static readonly BindableProperty SensorCellViewModelProperty = BindableProperty.Create(nameof(SensorCellViewModel), typeof(SensorCellViewModel), typeof(SensorCellView), null);

        public SensorCellViewModel SensorCellViewModel
        {
            get { return (SensorCellViewModel)GetValue(SensorCellViewModelProperty); }
            set { SetValue(SensorCellViewModelProperty, value); }
        }

        #endregion

        #region Constructor

        public SensorCellView()
        {
            InitializeComponent();

            this.SensorCellViewModel = Host.Current.GetService<SensorCellViewModel>();
        }

        #endregion

        #region Method         

        /// <summary>
        /// Click on image settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSettingsClickEvent(object sender, EventArgs e)
        {
            if (this.SensorCellViewModel != null)
            {
                if (this.SensorCellViewModel.SettingsSensorCommand.CanExecute(null))
                {
                    this.SensorCellViewModel.SettingsSensorCommand.Execute(this.BindingContext as Item);
                }
            }
        }

        #endregion
    }
}