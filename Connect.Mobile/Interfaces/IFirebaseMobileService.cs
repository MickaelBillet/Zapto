using System.Diagnostics;

namespace Connect.Mobile.Interfaces
{
    public interface IFirebaseMobileService
    {
        public void Initialize(object application);
        public void ReceiveNotification();
        public void Subscribe(string locationId);
        public void UnSubscribe();
        public void LinkToActivity(object activity, object intent);
    }
}
