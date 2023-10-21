using System.Collections.Generic;

namespace AirZapto.Model
{
	public interface INotificationProvider
	{
		#region Properties

        public string Id { get; set; }

        public IEnumerable<Notification> NotificationsList { get; set; }

		#endregion

		#region Methods

		#endregion
	}
}
