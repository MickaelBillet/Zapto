using Framework.Core.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Connect.Model
{
    public class ConnectedObject : Item, INotificationProvider
    {
        private string name = string.Empty;
        private ICollection<Notification> notificationsList = new ObservableCollection<Notification>();
        private Plug? plug = null;
        private Sensor? sensor = null;
        private int deviceType = 0;

        #region Property

        public string? RoomId { get; set; }

        public ICollection<Notification> NotificationsList
        {
            get { return notificationsList; }

            set { SetProperty<ICollection<Notification>>(ref notificationsList, value); }
        }

        public Plug? Plug
        {
            get { return plug; }

            set
            {
                SetProperty<Plug?>(ref plug, value);
            }
        }

        /// <summary>
        /// Allows to know the types of device present
        /// </summary>
        public int DeviceType
        {
            get { return deviceType; }

            set { SetProperty<int>(ref deviceType, value); }
        }

        public Sensor? Sensor
        {
            get { return sensor; }

            set
            {
                SetProperty<Sensor?>(ref sensor, value);
            }
        }

        public string Name
        {
            get { return name; }

            set { SetProperty<string>(ref name, value); }
        }

        #endregion

        #region Constructor

        public ConnectedObject() : base()
        {
            
        }

        #endregion

        #region Method              

        public static int CompareByDescription(ConnectedObject x, ConnectedObject y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    return x.Name.CompareTo(y.Name);
                }
            }
        }

        #endregion
    }
}
