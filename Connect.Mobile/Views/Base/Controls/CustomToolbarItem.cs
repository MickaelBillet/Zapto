﻿using Xamarin.Forms;

namespace Connect.Mobile.View.Controls
{
    public class CustomToolbarItem : ToolbarItem
    {
        public static readonly BindableProperty IsVisibleProperty =
                BindableProperty.Create(nameof(IsVisible),
                                        typeof(bool),
                                        typeof(CustomToolbarItem),
                                        true);
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }
    }
}
