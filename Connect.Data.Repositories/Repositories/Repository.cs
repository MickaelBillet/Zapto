using Connect.Data.DataContext;
using Framework.Core.Data;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Connect.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : ItemEntity
    {
		#region Properties
		protected ConnectContext? DataContext { get; } = null;
		protected DbSet<T>? Table { get; } = null;
        protected IDbConnection? DbConnection { get; } = null;
		#endregion

		#region Constructor
		public Repository(IDalSession session)
		{
			this.DataContext = session.DataContext as ConnectContext;
			this.Table = this.DataContext?.Set<T>();
            this.DbConnection = session.Connection;
		}
        #endregion

        #region Methods
        public async Task<IEnumerable<T>?> GetCollectionAsync()
        {
            return (this.Table != null) ? await this.Table.AsNoTracking().ToListAsync() : Enumerable.Empty<T>();
        }
        public async Task<int> InsertAsync(T entity)
        {
            int result = 0;

            if ((this.Table != null) && (this.DataContext != null))
            {
                await this.Table.AddAsync(entity);
                result = await this.DataContext.SaveChangesAsync();
            }

            return result;
        }
        public async Task<T?> GetAsync(string id)
        {
            T? entity = null;
            if (this.Table != null)
            {
                entity = await (from item in this.Table
                                where item.Id == id
                                select item).AsNoTracking().FirstOrDefaultAsync();
            }

            return entity;
        }
        public async Task<T?> GetAsync(Expression<Func<T, bool>>? condition = null,
                                            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            if ((this.DataContext != null) && (this.Table != null))
            {
                IQueryable<T> query = this.Table;
                if (condition != null)
                {
                    query = query.Where(condition).AsNoTracking();
                }

                if (orderBy != null)
                {
                    return await orderBy(query).FirstOrDefaultAsync();
                }
                else
                {
                    return await query.FirstOrDefaultAsync();
                }
            }

            return null;
        }
        public async Task<IEnumerable<T>?> GetCollectionAsync(Expression<Func<T, bool>>? condition = null, 
                                                                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            if ((this.DataContext != null) && (this.Table != null))
            {
                IQueryable<T> query = this.Table;
                if (condition != null)
                {
                    query = query.Where(condition).AsNoTracking();
                }

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }
                else
                {
                    return await query.ToListAsync();
                }
            }

            return null;
        }
        public async Task<int> DeleteAsync(T entity)
        {
            int res = 0;
            if ((this.DataContext != null) && (this.Table != null) && (this.Table.Any()))
            {
                //Search the entity in the local context 
                var existingEntity = this.Table.Local.FirstOrDefault(e => e.Id == entity.Id);
                if (existingEntity != null)
                {
                    //Detach the entity from the context
                    this.DataContext.Entry(existingEntity).State = EntityState.Detached;
                }

                //Rattach
                this.DataContext.Entry(entity).State = EntityState.Unchanged;
                this.Table.Remove(entity);
                res = await this.DataContext.SaveChangesAsync();
            }

            return res;
        }
        public async Task<bool> EntityExistsAsync(string id)
        {
            return (this.Table != null) ? (await this.Table.AsNoTracking().FirstOrDefaultAsync((arg) => arg.Id == id) != null) : false;
        }
        public async Task<int> UpdateAsync(T entity)
        {
            int res = 0;

            if (this.DataContext != null)
            {
                this.DataContext.DetachLocal<T>(entity, entity.Id);
                res = await this.DataContext.SaveChangesAsync();
            }

            return res;
        }
        public void Dispose()
		{
			this.DataContext?.Dispose();
		}

        public async Task<int> CreateTable(string sql)
        {
            int res = -1;

            try
            {
                if (this.DataContext != null)
                {
                    res = await this.DataContext.ExecuteNonQueryAsync(sql);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return res;
        }
  
        #endregion
    }
}
