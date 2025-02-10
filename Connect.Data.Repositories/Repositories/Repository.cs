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
        protected IDataContextFactory DataContextFactory { get; }
        #endregion

        #region Constructor
        public Repository(IDataContextFactory dataContextFactory)
		{
            this.DataContextFactory = dataContextFactory;
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<T>?> GetCollectionAsync()
        {
            IEnumerable<T>? result = null;
            await this.DataContextFactory.UseContext(async (context) =>
            {                
                DbSet<T>? table  = context?.Set<T>();
                if (table != null)
                {
                    result = await table.AsNoTracking().ToListAsync();
                }
            });
            return result;
        }
        public async Task<int> InsertAsync(T entity)
        {
            int result = 0;

            await this.DataContextFactory.UseContext(async (context) =>
            {
                DbSet<T>? table = context?.Set<T>();
                {
                    if ((table != null) && (context != null))
                    {
                        await table.AddAsync(entity);
                        result = await context.SaveChangesAsync();
                    }
                }
            });

            return result;
        }
        public async Task<T?> GetAsync(string id)
        {
            T? entity = null;

            await this.DataContextFactory.UseContext(async (context) =>
            {
                DbSet<T>? table = context?.Set<T>();
                if ((table != null) && (context != null))
                {
                    entity = await (from item in table
                                    where item.Id == id
                                    select item).AsNoTracking().FirstOrDefaultAsync();
                }
            });

            return entity;
        }
        public async Task<T?> GetAsync(Expression<Func<T, bool>>? condition = null,
                                            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            T? entity = null;
            await this.DataContextFactory.UseContext(async (context) =>
            {
                DbSet<T>? table = context?.Set<T>();
                if (table != null)
                {
                    IQueryable<T> query = table;
                    if (condition != null)
                    {
                        query = query.Where(condition).AsNoTracking();
                    }

                    if (orderBy != null)
                    {
                        entity = await orderBy(query).FirstOrDefaultAsync();
                    }
                    else
                    {
                        entity = await query.FirstOrDefaultAsync();
                    }
                }
            });

            return entity;
        }
        public async Task<IEnumerable<T>?> GetCollectionAsync(Expression<Func<T, bool>>? condition = null, 
                                                                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IEnumerable<T>? result = null;
            await this.DataContextFactory.UseContext(async (context) =>
            {
                DbSet<T>? table = context?.Set<T>();
                if (table != null)
                {
                    IQueryable<T> query = table;
                    if (condition != null)
                    {
                        query = query.Where(condition).AsNoTracking();
                    }

                    if (orderBy != null)
                    {
                        result = await orderBy(query).ToListAsync();
                    }
                    else
                    {
                        result = await query.ToListAsync();
                    }
                }
            });
            return result;
        }
        public async Task<int> DeleteAsync(T entity)
        {
            int res = 0;
            await this.DataContextFactory.UseContext(async (context) =>
            {
                DbSet<T>? table = context?.Set<T>();
                if ((context != null) && (table != null) && (table.Any()))
                {
                    //Search the entity in the local context 
                    var existingEntity = table.Local.FirstOrDefault(e => e.Id == entity.Id);
                    if (existingEntity != null)
                    {
                        //Detach the entity from the context
                        context.Entry(existingEntity).State = EntityState.Detached;
                    }

                    //Rattach
                    context.Entry(entity).State = EntityState.Unchanged;
                    table.Remove(entity);
                    res = await context.SaveChangesAsync();
                }
            });
            return res;
        }
        public async Task<bool> EntityExistsAsync(string id)
        {
            bool result = false;
            await this.DataContextFactory.UseContext(async (context) =>
            {
                DbSet<T>? table = context?.Set<T>();
                if (table != null)
                {
                    result = (await table.AsNoTracking().FirstOrDefaultAsync((arg) => arg.Id == id) != null);
                }
            });
            return result;
        }
        public async Task<int> UpdateAsync(T entity)
        {
            int res = 0;
            await this.DataContextFactory.UseContext(async (context) =>
            {
                if (context != null)
                {
                    context.DetachLocal<T>(entity, entity.Id);
                    res = await context.SaveChangesAsync();
                }
            });
            return res;
        }
        public void Dispose()
		{
		}

        public async Task<int> CreateTable(string sql)
        {
            int res = -1;

            try
            {
                await this.DataContextFactory.UseContext(async (context) =>
                {
                    if (context != null)
                    {
                        res = await context.ExecuteNonQueryAsync(sql);
                    }
                });
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
