using Framework.Core.Base;
using System;
using System.Data;

namespace Framework.Data.Abstractions
{
    public interface IDalSession : IDisposable
    {
        public IDataContext? DataContext { get; }
        public IDbConnection? Connection { get; }
        public ConnectionType? ConnectionType { get; }
        public bool OpenConnection();
        public void CloseConnection();
    }
}
