using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace AirZapto.View.Controls
{
	public class BasePage : ContentPage
    {
        #region Property

        public IList<CustomToolbarItem> CustomToolbar { get; private set; }

        #endregion

        #region Conrtructor

        public BasePage()
        {
            ObservableCollection<CustomToolbarItem> items = new ObservableCollection<CustomToolbarItem>();
            items.CollectionChanged += ToolbarItemsChanged;

            CustomToolbar = items;
        }

        #endregion

        #region Method

        private void ToolbarItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ToolbarItems.Clear();

            foreach (CustomToolbarItem item in CustomToolbar)
            {
                item.PropertyChanged += OnToolbarItemPropertyChanged;

                if (item.IsVisible)
                {
                    ToolbarItems.Add(item);
                }
            }
        }

        private void OnToolbarItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CustomToolbarItem.IsVisibleProperty.PropertyName)
            {
                UpdateToolbar(sender as CustomToolbarItem);
            }
        }

        public void UpdateToolbar(CustomToolbarItem item)
        {
            if (item.IsVisible)
            {
                if (!ToolbarItems.Contains(item))
                {
                    ToolbarItems.Add(item);
                }
            }
            else
            {
                if (ToolbarItems.Contains(item))
                {
                    ToolbarItems.Remove(item);
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        #endregion
    }
}
