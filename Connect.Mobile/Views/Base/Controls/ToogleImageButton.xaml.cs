using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Connect.Mobile.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ToogleImageButton : ContentView
	{
        #region Properties

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            propertyName: nameof(Command),
            returnType: typeof(ICommand), 
            declaringType: typeof(ToogleImageButton),
            null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(CommandParameter), 
            returnType: typeof(object), 
            declaringType: typeof(ToogleImageButton), 
            null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static BindableProperty TextCheckedProperty = BindableProperty.Create(
            propertyName: nameof(TextChecked),
            returnType: typeof(string),
            declaringType: typeof(ToogleImageButton),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay);

        public string TextChecked
        {
            get { return (string)base.GetValue(TextCheckedProperty); }
            set
            {
                if (value != this.TextChecked)
                    base.SetValue(TextCheckedProperty, value);
            }
        }

        public static BindableProperty TextUnCheckedProperty = BindableProperty.Create(
            propertyName: nameof(TextUnChecked),
            returnType: typeof(string),
            declaringType: typeof(ToogleImageButton),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay);

        public string TextUnChecked
        {
            get { return (string)base.GetValue(TextUnCheckedProperty); }
            set
            {
                if (value != this.TextUnChecked)
                    base.SetValue(TextUnCheckedProperty, value);
            }
        }

        public static BindableProperty ImageUnCheckedProperty = BindableProperty.Create(
            propertyName: nameof(ImageUnChecked),
            returnType: typeof(ImageSource),
            declaringType: typeof(ToogleImageButton),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay);

        public ImageSource ImageUnChecked
        {
            get { return (ImageSource)base.GetValue(ImageUnCheckedProperty); }
            set
            {
                if (value != this.ImageUnChecked)
                    base.SetValue(ImageUnCheckedProperty, value);
            }
        }

        public static BindableProperty ImageCheckedProperty = BindableProperty.Create(
            propertyName: nameof(ImageChecked),
            returnType: typeof(ImageSource),
            declaringType: typeof(ToogleImageButton),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay);

        public ImageSource ImageChecked
        {
            get { return (ImageSource)base.GetValue(ImageCheckedProperty); }
            set
            {
                if (value != this.ImageChecked)
                    base.SetValue(ImageCheckedProperty, value);
            }
        }

        public static BindableProperty IsCheckedProperty = BindableProperty.Create(
            propertyName: nameof(IsChecked),
            returnType: typeof(bool?),
            declaringType: typeof(ToogleImageButton),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: IsCheckedPropertyChanged);

        public bool? IsChecked
        {
            get { return (bool?)base.GetValue(IsCheckedProperty); }
            set
            {
                if (value != this.IsChecked)
                    base.SetValue(IsCheckedProperty, value);
            }
        }

		#endregion

		#region Constructor

		public ToogleImageButton()
		{
			InitializeComponent();

		}

		#endregion

		#region Methods

		private void ImageButton_Clicked(object sender, EventArgs e)
		{
          //  IsChecked = !IsChecked;
		}

        private static void IsCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ToogleImageButton targetView;

            targetView = (ToogleImageButton)bindable;

            if (targetView != null)
            {
                targetView.Button.Source = (targetView.IsChecked == true) ? targetView.ImageChecked : targetView.ImageUnChecked;
                targetView.Label.Text = (targetView.IsChecked == true) ? targetView.TextChecked : targetView.TextUnChecked;
            }
        }

		#endregion
	}
}