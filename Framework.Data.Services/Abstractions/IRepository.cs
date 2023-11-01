using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Data.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<int> InsertAsync(T entity);
        Task<IEnumerable<T>?> GetCollectionAsync(Expression<Func<T, bool>>? condition = null,
                                                    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<T?> GetAsync(string id);
        Task<int> DeleteAsync(T entity);
        Task<bool> EntityExists(string id);
        Task<int> UpdateAsync(T entity);
        Task<T?> GetAsync(Expression<Func<T, bool>>? condition = null,
                            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    }
}
