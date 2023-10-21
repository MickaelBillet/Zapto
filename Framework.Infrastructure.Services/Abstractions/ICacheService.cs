using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface ICacheService
	{
		Task InsertObject<T>(string name, T item);
		Task<IEnumerable<T>> GetAllObjects<T>();
		Task<T> GetObject<T>(string ident);
	}
}
