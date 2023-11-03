﻿using Framework.Core.Base;
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
		protected IDataContextFactory? DataContextFactory { get; }
		protected IConfiguration Configuration { get; }
		public bool IsInitialized { get; private set; }
		#endregion

		#region Constructor
		public DatabaseService(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
		{
			this.DataContextFactory = serviceProvider.GetRequiredService<IDataContextFactory>();
			this.Configuration = configuration;
		}
        #endregion

        #region Methods

        public virtual async Task InitializeDataAsync()
        {
            this.IsInitialized = await Task.FromResult<bool>(true);
        }

        public virtual async Task<bool> UpgradeDatabaseAsync()
        {
            return await Task.FromResult<bool>(true);
        }

        public bool DropDatabase(ConnectionType connectionType)
		{
			bool res = false;

			if (this.DataContextFactory != null)
			{
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

		public bool CreateDatabase(ConnectionType connectionType)
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

		public bool DatabaseExist(ConnectionType connectionType)
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

		public virtual async Task FeedDataAsync()
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