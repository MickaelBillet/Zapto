using System;

using Xamarin.Forms;

namespace Connect.Mobile.View.Controls
{
	public class CustomPicker : Picker
	{
		public static readonly BindableProperty SelectedFontSizeProperty =
									BindableProperty.Create("SelectedFontSize",
                                    typeof(float),
                                    typeof(CustomPicker),
                                    0f);

		public float SelectedFontSize
		{
            get { return (float)GetValue(SelectedFontSizeProperty); }
			set { SetValue(SelectedFontSizeProperty, value); }
		}
	}
}

