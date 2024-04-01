using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Framework.Data.Services
{
    public class DatabaseService : IDatabaseService
	{
		private bool _disposed = false;

		#region Properties
		protected IDataContextFactory DataContextFactory { get; }
        protected IServiceScopeFactory ServiceScopeFactory { get; set; }
        protected IConfiguration Configuration { get; }
        private bool IsInitialized { get; set; }
		#endregion

		#region Constructor
		public DatabaseService(IDataContextFactory dataContextFactory, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
		{
			this.DataContextFactory = dataContextFactory;
            this.ServiceScopeFactory = serviceScopeFactory;
            this.Configuration = configuration;
		}
        #endregion

        #region Methods

        public async Task ConfigureDatabase()
        {
            ConnectionType connectionType = new()
            {
                ConnectionString = this.Configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(this.Configuration["ConnectionStrings:ServerType"])
            };

            if (this.DatabaseExist(connectionType) == false)
            {
                bool isCreated = this.CreateDatabase(connectionType);
                if (isCreated == true)
                {
                    await this.FeedDataAsync();
                }
            }

            if (this.DatabaseExist(connectionType) == true)
            {
                bool isUpgraded = await this.UpgradeDatabaseAsync();
                await this.InitializeDataAsync();
            }
        }

        protected virtual async Task InitializeDataAsync()
        {
            this.IsInitialized = await Task.FromResult<bool>(true);
        }

        protected virtual async Task<bool> UpgradeDatabaseAsync()
        {
            return await Task.FromResult<bool>(true);
        }

        public bool DropDatabase()
		{
			bool res = false;

			if (this.DataContextFactory != null)
			{
                ConnectionType connectionType = new()
                {
                    ConnectionString = this.Configuration["ConnectionStrings:DefaultConnection"],
                    ServerType = ConnectionType.GetServerType(this.Configuration["ConnectionStrings:ServerType"])
                };

                using (IDataContext? context = this.DataContextFactory.CreateDbContext(connectionType.ConnectionString, connectionType.ServerType)?.context)
				{
					if ((context != null) && (context.DataBaseExists() == true))
					{
						res = context.DropDatabase();
					}
				}
			}

			return res;
		}

        protected bool CreateDatabase(ConnectionType connectionType)
		{
			bool res = false;

			if (this.DataContextFactory != null)
			{
                using (IDataContext? context = this.DataContextFactory.CreateDbContext(connectionType.ConnectionString, connectionType.ServerType)?.context)
                {
                    if (context != null)
                    {
                        res = context.CreateDataBase();
					}
				}
			}

			return res;
		}

        protected bool DatabaseExist(ConnectionType connectionType)
		{
			bool res = false;

			if (this.DataContextFactory != null)
			{
                using (IDataContext? context = this.DataContextFactory.CreateDbContext(connectionType.ConnectionString, connectionType.ServerType)?.context)
                {
					if (context != null)
					{
						res = context.DataBaseExists();
					}
				}
			}

			return res;
		}

        public bool DatabaseIsInitialized()
		{
			return this.IsInitialized;
		}

        protected virtual async Task FeedDataAsync()
		{
			await Task.FromResult(0);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				if (_disposed)
					return;

				if (disposing)
				{

				}

				_disposed = true;
			}

			_disposed = true;
		}

		#endregion
	}
}
