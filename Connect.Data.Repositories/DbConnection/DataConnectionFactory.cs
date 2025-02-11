using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using System.Data;

namespace Connect.Data.DataConnection
{
    internal class DataConnectionFactory : IDataConnectionFactory
    {
        #region Properties
        protected ConnectionType ConnectionType { get; }
        #endregion

        #region Constructor
        public DataConnectionFactory(ISecretService secretService, string connectionStringKey, string serverTypeKey)
        {
            this.ConnectionType = ConnectionString.GetConnectionType(secretService, connectionStringKey, serverTypeKey);
        }
        #endregion

        #region Methods
        public IDbConnection? CreateDbConnection()
        {
            IDbConnection? connection = null;

            if (this.ConnectionType.ServerType == ServerType.MySql)
            {
                connection = new MySqlConnection(this.ConnectionType.ConnectionString);
            }
            else if (this.ConnectionType.ServerType == ServerType.SqlLite)
            {
                connection = new SqliteConnection(this.ConnectionType.ConnectionString);
            }

            return connection;
        }
        #endregion
    }
}
