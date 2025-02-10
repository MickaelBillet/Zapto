using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Abstractions;
using Framework.Infrastructure.Services;
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
		protected IDataContextFactory? DataContextFactory { get; }
        protected IServiceScopeFactory? ServiceScopeFactory { get; }
		protected ISecretService? SecretService { get; }
        protected IConfiguration? Configuration { get; }
		protected ConnectionType ConnectionType { get; }
        private bool IsInitialized { get; set; }
		#endregion

		#region Constructor
		public DatabaseService(IServiceProvider serviceProvider, string connectionStringKey, string serverTypeKey)
		{
			this.DataContextFactory = serviceProvider.GetRequiredService<IDataContextFactory>();
            this.ServiceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            this.Configuration = serviceProvider.GetRequiredService<IConfiguration>();
			this.SecretService = serviceProvider.GetRequiredService<ISecretService>();
			this.ConnectionType = ConnectionString.GetConnectionType(this.SecretService, connectionStringKey, serverTypeKey); 
		}
        public DatabaseService(IServiceProvider serviceProvider)
        {
            this.DataContextFactory = serviceProvider.GetRequiredService<IDataContextFactory>();
            this.ServiceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            this.Configuration = serviceProvider.GetRequiredService<IConfiguration>();
            this.ConnectionType = ConnectionString.GetConnectionType(this.Configuration);
        }
        #endregion

        #region Methods
        public async Task ConfigureDatabase(int major, int minor, int build)
        {		
            if ((await this.DatabaseExistAsync(this.ConnectionType)) == false)
            {
                bool isCreated = await this.CreateDatabaseAsync();
                if (isCreated == true)
                {
                    await this.FeedDataAsync(major, minor, build);
                }
            }

            if ((await this.DatabaseExistAsync(this.ConnectionType)) == true)
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

        public async Task<bool> DropDatabaseAsync()
		{
			bool res = false;
			if (this.DataContextFactory != null)
			{
				await this.DataContextFactory.UseContext(async context =>
				{
					if ((context != null) && ((await context.DataBaseExistsAsync()) == true))
					{
						bool? result = await context.DataBaseExistsAsync();
                        res = result.HasValue ? result.Value : false;
                    }
				});
			}
			return res;
		}

        protected async Task <bool> CreateDatabaseAsync()
		{
			bool res = false;
			if (this.DataContextFactory != null)
			{
				await this.DataContextFactory.UseContext(async context =>
				{
					if (context != null)
					{
						res = await context.CreateDataBaseAsync();
					}
				});
			}
			return res;
		}

        protected async Task<bool> DatabaseExistAsync(ConnectionType connectionType)
		{
			bool res = false;
			if (this.DataContextFactory != null)
			{
                await this.DataContextFactory.UseContext(async context =>
                {
                    if (context != null)
                    {
                        res = await context.DataBaseExistsAsync();
                    }
                });
            }
			return res;
		}

        public bool DatabaseIsInitialized()
		{
			return this.IsInitialized;
		}

        protected virtual async Task FeedDataAsync(int major, int minor, int build)
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
