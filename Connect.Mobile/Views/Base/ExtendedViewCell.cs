using System;

using Xamarin.Forms;

namespace Connect.Mobile.View
{
	public class ExtendedViewCell : ViewCell
	{
		public static readonly BindableProperty SelectedBackgroundColorProperty =
			                        BindableProperty.Create("SelectedBackgroundColor",
									typeof(Color),
									typeof(ExtendedViewCell),
									Color.Default);

		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set { SetValue(SelectedBackgroundColorProperty, value); }
		}
	}

}

