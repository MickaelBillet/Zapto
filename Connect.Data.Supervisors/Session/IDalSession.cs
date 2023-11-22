using Framework.Core.Base;
using Framework.Data.Abstractions;
using System.Data;

namespace Connect.Data.Session
{
    public interface IDalSession : IDisposable
    {
        public IDataContext DataContext { get; }
        public IDbConnection Connection { get; }
        public ConnectionType ConnectionType { get; }
        public bool OpenConnection();
        public void CloseConnection();
    }
}
