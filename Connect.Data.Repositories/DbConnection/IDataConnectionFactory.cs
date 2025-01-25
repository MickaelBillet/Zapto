using System.Data;

namespace Connect.Data.DataConnection
{
    public interface IDataConnectionFactory
    {
        public IDbConnection? CreateDbConnection();
    }
}