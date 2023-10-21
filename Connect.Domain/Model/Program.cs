using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Framework.Core.Domain;

namespace Connect.Model
{
    public class Program : Item
    {
        private ObservableCollection<OperationRange>? operationRangeList = new ObservableCollection<OperationRange>();

        #region Property

        public string ConnectedObjectId { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public ObservableCollection<OperationRange>? OperationRangeList
        {
            get { return operationRangeList; }

            set { SetProperty<ObservableCollection<OperationRange>?>(ref operationRangeList, value); }
        }

        #endregion

        #region Constructor

        public Program(): base() 
        {
        }

        #endregion

        #region Method
              
        #endregion
    }
}
