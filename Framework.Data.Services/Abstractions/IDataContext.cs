using Framework.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Data.Abstractions
{
    public interface IDataContext  : IDisposable
	{
		Task<bool> DropDatabaseAsync();
		Task<bool> CreateDataBaseAsync();
		Task<bool> DataBaseExistsAsync();
		void DetachLocal<T>(T t, string entryId) where T : ItemEntity;
		DbSet<T> Set<T>() where T : class;
		Task<int> ExecuteNonQueryAsync(string sql);
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
