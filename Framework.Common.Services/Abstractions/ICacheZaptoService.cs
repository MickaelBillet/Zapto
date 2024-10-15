using Framework.Core.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace Framework.Infrastructure.Services
{
    public interface ICacheZaptoService<T> where T : class
	{
        Task<(ResultCode, T)> GetOrCreate(string key, Func<string, Task<(ResultCode, T)>> createItem);
        Task Set(string key, T value);
        Task<IEnumerable<T>> GetAll(Func<T, bool> func = null);
        Task<T> Get(Func<T, bool> func);
        Task<ResultCode> Delete(string key);
    }
}
