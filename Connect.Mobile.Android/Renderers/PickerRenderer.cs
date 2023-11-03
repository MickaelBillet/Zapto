using Connect.Mobile.Droid.Renderers;
using Connect.Mobile.View.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(PickerDroidRenderer))]


namespace Connect.Mobile.Droid.Renderers
{
    public class PickerDroidRenderer : PickerRenderer
    {
        #region Property

        #endregion

        #region Constructor

        public PickerDroidRenderer(Android.Content.Context context) : base(context)
        {
        }

        #endregion

        #region Method

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (this.Element != null)
            {
                CustomPicker picker = (CustomPicker)this.Element;

                if (picker != null)
                {
                    this.Control.TextSize = ((float)picker.SelectedFontSize);
                    this.Control.TextAlignment = Android.Views.TextAlignment.Center;
                }
            }
        }

        #endregion
    }
}