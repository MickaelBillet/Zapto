using System.Collections.Generic;
using System.Collections.ObjectModel;
using Framework.Core.Domain;

namespace Connect.Model
{
    public class Location : Item
    { 
        private string address = string.Empty;
        private string zipcode = string.Empty;
        private string city = string.Empty;
        private string country = string.Empty;
        private string description = string.Empty;

        private IEnumerable<Room>? roomsList = null;

        #region Property

        public string UserId { get; set; } = string.Empty;

        public string Address
        {
            get { return address;  }

            set { SetProperty<string>(ref address, value); }
        }

        public string Zipcode
        {
            get { return zipcode; }

            set { SetProperty<string>(ref zipcode, value); }
        }

        public string Country
        {
            get { return country; }

            set { SetProperty<string>(ref country, value); }
        }

        public string City
        {
            get { return city; }

            set { SetProperty<string>(ref city, value); }
        }

        public string Description
        {
            get { return description; }

            set { SetProperty<string>(ref description, value); }
        }

        public IEnumerable<Room>? RoomsList
        {
            get { return roomsList; }

            set { SetProperty<IEnumerable<Room>?>(ref roomsList, value); }
        }

        #endregion

        #region Constructor

        public Location() : base()
        {
            this.RoomsList = new ObservableCollection<Room>();
        }

        #endregion

        #region Methods

        #endregion
    }
}
