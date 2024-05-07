using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Framework.Data.Session
{
    public sealed class DalSession : IDalSession
    {
        #region Properties

        public IDbConnection? Connection
        {
            get; private set;
        }

        public IDataContext? DataContext
        {
            get; private set;
        }

        public ConnectionType? ConnectionType 
        { 
            get; private set; 
        }

        #endregion

        #region Constructor

        public DalSession(IKeyVaultService keyVaultService, IDataContextFactory dataContextFactory, IConfiguration configuration)
        {
#if DEBUG
            this.ConnectionType = new()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"])
            };
#else
            this.ConnectionType = new()
            {
                ConnectionString = keyVaultService.GetSecret("ConnectionStrings"),
                ServerType = ConnectionType.GetServerType(keyVaultService.GetSecret("ServerType"))
            };
#endif

            if (string.IsNullOrEmpty(this.ConnectionType?.ConnectionString) == false)
            {
                (IDbConnection? connection, IDataContext? context)? obj = dataContextFactory.CreateDbContext(this.ConnectionType.ConnectionString, this.ConnectionType.ServerType);
                if (obj != null)
                {
                    this.Connection = obj?.connection;
                    this.DataContext = obj?.context;
                }
            }
        }

        #endregion

        #region Methods

        public bool OpenConnection()
        {
            bool res = false;

            this.Connection?.Open();

            if (this.Connection?.State == ConnectionState.Open)
            {
                res = true;
            }

            return res;
        }

        public void CloseConnection()
        {
            this.Connection?.Close();
        }

        public void Dispose()
        {
            this.Connection?.Close();

            this.DataContext?.Dispose();
            this.Connection?.Dispose();
        }

        #endregion
    }
}
