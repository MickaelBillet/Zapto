using Akavache;
using Framework.Infrastructure.Services;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Connect.Mobile.Services
{
    internal class CacheService : ICacheService
	{
		#region Constructor

		#endregion

		#region Methods

		public async Task InsertObject<T>(string ident, T item)
		{
			await BlobCache.LocalMachine.InsertObject<T>(ident, item);
		}

		public async Task<IEnumerable<T>> GetAllObjects<T>()
		{
			return await BlobCache.LocalMachine.GetAllObjects<T>();
		}

		public async Task<T> GetObject<T>(string ident)
		{
			return await BlobCache.LocalMachine.GetObject<T>(ident);
		}

		#endregion
	}
}
