using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Connect.Mobile.View.Controls
{
    public class ImageButton : Image
    {
        #region Event

        public event EventHandler TapEvent;

        #endregion

        #region Property

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ImageButton), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(ImageButton), null);

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

        private ICommand TransitionCommand
        {
            get
            {
                return new Command(async () =>
                {
                    this.AnchorX = 0.48;
                    this.AnchorY = 0.48;
                    await this.ScaleTo(0.8, 50, Easing.Linear);
                    await Task.Delay(100);
                    await this.ScaleTo(1, 50, Easing.Linear);

                    if (this.TapEvent != null)
                    {
                        this.TapEvent.Invoke(this, new EventArgs());
                    }

                    if (Command != null)
                    {
                        this.Command.Execute(CommandParameter);
                    }
                });
            }
        }

        #endregion

        #region Constructor

        public ImageButton()
        {
            Initialize();
        }

        #endregion

        #region Method

        public void Initialize()
        {
            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = TransitionCommand,                
            });
        }

        protected virtual void OnTapEvent(EventArgs e)
        {
            EventHandler handler = TapEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
