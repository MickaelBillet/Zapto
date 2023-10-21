using System.Collections.Generic;

namespace Connect.Model
{
    public interface INotificationProvider
	{
		#region Properties

		/// <summary>
		/// Allows to know the types of device present
		/// </summary>
		public int DeviceType { get; set; }

        public string Id { get; set; }

        public ICollection<Notification> NotificationsList { get; set; }

		#endregion

		#region Methods

		#endregion
	}
}
