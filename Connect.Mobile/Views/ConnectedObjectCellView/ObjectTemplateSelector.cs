using Connect.Model;
using Xamarin.Forms;

namespace Connect.Mobile.View
{
    class ObjectTemplateSelector : DataTemplateSelector
    {
		#region Properties

		public DataTemplate PlugTemplate { get; set; }

        public DataTemplate SensorTemplate { get; set; }

		#endregion

		#region Methods

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            ConnectedObject connectedObject = item as ConnectedObject;

            if ((connectedObject != null) && (connectedObject.Plug != null))
            {
                return this.PlugTemplate;
            }
            else if ((connectedObject != null) && (connectedObject.Sensor != null))
            {
                return this.SensorTemplate;
            }

            return null;
        }

		#endregion
	}
}
