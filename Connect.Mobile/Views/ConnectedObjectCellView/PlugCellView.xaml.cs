using Connect.Mobile.ViewModel;
using Framework.Core.Domain;
using System;
using Xamarin.Forms;

namespace Connect.Mobile.View
{
    [Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
    public partial class PlugCellView : ContentView
    {
        #region Property

        public static readonly BindableProperty PlugCellViewModelProperty = BindableProperty.Create("PlugCellViewModel", typeof(PlugCellViewModel), typeof(PlugCellView), null);

        public PlugCellViewModel PlugCellViewModel
        {
            get { return (PlugCellViewModel)GetValue(PlugCellViewModelProperty); }
            set { SetValue(PlugCellViewModelProperty, value); }
        }

        #endregion

        #region Constructor

        public PlugCellView()
        {
            InitializeComponent();

            this.PlugCellViewModel = Host.Current.GetService<PlugCellViewModel>();
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
            if (this.PlugCellViewModel != null)
            {
                if (this.PlugCellViewModel.SettingsPlugCommand.CanExecute(null))
                {
                    this.PlugCellViewModel.SettingsPlugCommand.Execute(this.BindingContext as Item);
                }
            }
        }

        #endregion
    }
}