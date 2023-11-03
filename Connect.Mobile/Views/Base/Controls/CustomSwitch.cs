using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Connect.Mobile.View.Controls
{
    class CustomSwitch : Switch
    {
        #region Properties

        public static readonly BindableProperty StatusProperty = BindableProperty.CreateAttached(propertyName: nameof(Status),
                                                                                                    returnType: typeof(bool),
                                                                                                    declaringType: typeof(CustomSwitch),
                                                                                                    defaultValue: false,
                                                                                                    defaultBindingMode: BindingMode.TwoWay,
                                                                                                    validateValue: null,
                                                                                                    propertyChanged: OnStatusChanged);

        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), 
                                                                                            typeof(ICommand), 
                                                                                            typeof(CustomSwitch), null);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), 
                                                                                                    typeof(object), 
                                                                                                    typeof(CustomSwitch), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion

        #region Constructor

        public CustomSwitch():base()
        {
            this.Toggled += CustomSwitchToggled;
        }

        #endregion

        #region Methods

        public void CustomSwitchToggled(object sender, ToggledEventArgs e)
        {
            this.Status = this.IsToggled;

            this.Command?.Execute(this.CommandParameter);
        }

        static void OnStatusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomSwitch obj = bindable as CustomSwitch;

            if ((obj != null) && (obj.IsToggled != obj.Status))
            {
                obj.Toggled -= obj.CustomSwitchToggled;
                obj.IsToggled = obj.Status;
                obj.Toggled += obj.CustomSwitchToggled;
            }
        }

        #endregion        
    }
}
