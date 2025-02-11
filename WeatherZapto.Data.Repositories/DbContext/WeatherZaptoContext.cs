using Framework.Core.Data;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WeatherZapto.Data.Entities;

namespace WeatherZapto.Data.DataContext
{
    public abstract class WeatherZaptoContext : DbContext, IDataContext
	{
		#region Properties
		public DbSet<VersionEntity> Versions { get; set; }
		public DbSet<LogsEntity> LogsEntities { get; set; }
		public DbSet<AirPollutionEntity> AirPollutionEntities { get; set; }
		public DbSet<WeatherEntity> WeatherEntities { get; set; }
		public DbSet<CallEntity> CallEntities { get; set; }
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

		public WeatherZaptoContext(IDbConnection connection) : base()
		{
			this.Connection = connection;
		}

		public WeatherZaptoContext(DbContextOptions options) : base(options)
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
			if (this.Transaction != null)
				this.Transaction.Dispose();

			this.Transaction = null;

			base.Dispose();
		}

        public virtual async Task<int> ExecuteNonQueryAsync(string sql) => await Task.FromResult<int>(-1);

        #endregion
    }
}
