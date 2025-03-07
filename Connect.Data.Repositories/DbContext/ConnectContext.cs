﻿using Connect.Data.Entities;
using Framework.Core.Data;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Connect.Data.DataContext
{
    public abstract class ConnectContext : DbContext, IDataContext
	{
		#region Properties
		public DbSet<VersionEntity> Versions { get; set; }
		public DbSet<ClientAppEntity> ClientAppsEntities { get; set; }
		public DbSet<NotificationEntity> NotificationEntities { get; set; }
		public DbSet<SensorEntity> SensorEntities { get; set; }
		public DbSet<ConditionEntity> SensorDataEntities { get; set; }
		public DbSet<LogsEntity> LogsEntities { get; set; }
		public DbSet<ConditionEntity> ConditionEntities { get; set; }
		public DbSet<ConfigurationEntity> ConfigurationEntities { get; set; }
		public DbSet<ConnectedObjectEntity> ConnectedObjectEntities { get; set; }
		public DbSet<LocationEntity> LocationEntities { get; set; }
		public DbSet<OperatingDataEntity> OperatingDataEntities { get; set; }
		public DbSet<OperationRangeEntity> OperationRangesEntities { get; set; }
		public DbSet<PlugEntity> PlugEntities { get; set; }
		public DbSet<ProgramEntity> ProgramEntities { get; set; }
		public DbSet<RoomEntity> RoomEntities { get; set; }
		public DbSet<ServerIotStatusEntity> ServerIotStatusEntities { get; set; }		

		public IDbConnection? Connection
		{
			get; private set;
		}

		public IDbTransaction? Transaction
		{
			get; private set;
		}

		#endregion

		#region Constructor

		public ConnectContext(IDbConnection connection) : base()
		{
			this.Connection = connection;
		}

		public ConnectContext(DbContextOptions options) : base(options)
		{

		}

        #endregion

        #region Methods

        public void DetachLocal<T>(T t, string entryId) where T : ItemEntity
		{
			var local = this.Set<T>()
				.Local
				.FirstOrDefault(entry => entry.Id.Equals(entryId));

			if (local != null)
			{
				this.Entry(local).State = EntityState.Detached;
			}

			this.Entry(t).State = EntityState.Modified;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}

		public async Task<bool> DropDatabaseAsync()
		{
			return await this.Database.EnsureDeletedAsync();
		}

		public async Task<bool> CreateDataBaseAsync()
		{
			return await this.Database.EnsureCreatedAsync();
		}

		public async Task<bool> DataBaseExistsAsync()
		{
			return await this.Database.CanConnectAsync();
		}

		public override void Dispose()
		{
			this.Transaction?.Dispose();

			this.Transaction = null;

			base.Dispose();
		}

		public virtual async Task<int> ExecuteNonQueryAsync(string sql) => await Task.FromResult<int>(-1);

        #endregion
    }
}
