﻿using AirZapto.Data.Entities;
using Framework.Core.Data;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AirZapto.Data.DataContext
{
    public abstract class AirZaptoContext : DbContext, IDataContext
    {
        #region Properties
        public DbSet<SensorEntity> SensorEntities { get; set; }
        public DbSet<SensorDataEntity> SensorDataEntities { get; set; }
        public DbSet<LogsEntity> LogsEntities { get; set; }
        public DbSet<VersionEntity> VersionEntities { get; set; }

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

        public AirZaptoContext(IDbConnection connection)
        {
            this.Connection = connection;
        }

        #endregion

        #region Methods

        public void DetachLocal<T>(T t, string entryId) where T : ItemEntity
        {
            var local = this.Set<T>().Local.FirstOrDefault(entry => entry.Id.Equals(entryId));

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
            if (this.Transaction != null)
                this.Transaction.Dispose();

            this.Transaction = null;

            base.Dispose();
        }

        public virtual async Task<int> ExecuteNonQueryAsync(string sql) => await Task.FromResult<int>(-1);

        #endregion
    }
}
