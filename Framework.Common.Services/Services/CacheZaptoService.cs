using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Framework.Common.Services
{
    public sealed class CacheZaptoService<T> : ICacheZaptoService<T> where T : class
    {
        #region Properties
        private IDictionary<string, T> Cache { get; }
        private CacheSignal CacheSignal { get; }
        #endregion

        #region Constructor
        public CacheZaptoService(IServiceProvider serviceProvider) 
        {
            this.Cache = new Dictionary<string, T>();
            this.CacheSignal = serviceProvider.GetRequiredService<CacheSignal>();
        }
        #endregion

        #region Methods
        public async Task<(ResultCode, T)> GetOrCreate(string key, Func<string, Task<(ResultCode, T)>> createItem)
        {
            T cacheEntry = null;
            ResultCode code = ResultCode.Ok;

            if (this.CacheSignal != null && this.Cache != null)
            {
                await this.CacheSignal.WaitAsync();
                
                try
                {
                    if (this.Cache.TryGetValue(key, out cacheEntry) == false)// Look for cache key.
                    {
                        // Key not in cache, so get data.
                        (code, cacheEntry) = await createItem(key);
                        this.Cache.Add(key, cacheEntry);
                    }
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
            return (code, cacheEntry);
        }
        public async Task Set(string key, T value) 
        {
            if (this.CacheSignal != null && this.Cache != null)
            {
                await this.CacheSignal.WaitAsync();

                try
                {
                    if (this.Cache.ContainsKey(key))
                    {
                        this.Cache[key] = value;
                    }
                    else
                    {
                        this.Cache.Add(key, value);
                    }
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
        }
        public async Task<IEnumerable<T>> GetAll(Func<T, bool> func = null)
        {
            IEnumerable<T> cacheEntry = null;

            if (this.CacheSignal != null && this.Cache != null)
            {
                await this.CacheSignal.WaitAsync();

                try
                {
                    cacheEntry = (func != null) ? this.Cache.Values.Where((item) => func(item)) : this.Cache.Values;
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
            return cacheEntry;
        }
        public async Task<T> Get(Func<T, bool> func)
        {
            T cacheEntry = null;

            if (this.CacheSignal != null && this.Cache != null)
            {
                await this.CacheSignal.WaitAsync();

                try
                {
                    cacheEntry = this.Cache.Values.SingleOrDefault((item) => func(item));
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
            return cacheEntry;
        }
        public async Task<ResultCode> Delete(string key) 
        {
            ResultCode code = ResultCode.CouldNotDeleteItem;
            if (this.CacheSignal != null && this.Cache != null)
            {
                await this.CacheSignal.WaitAsync();

                try
                {
                    code = (this.Cache.Remove(key) == true) ? ResultCode.Ok: ResultCode.CouldNotDeleteItem;
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
            return code;
        }
        #endregion
    }
}
