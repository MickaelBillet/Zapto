using Connect.Model;
using Xamarin.Forms;

namespace Connect.Mobile.View
{
    class RoomTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RoomTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Room)
            {
                return this.RoomTemplate;
            }

            return null;
        }
    }
}
