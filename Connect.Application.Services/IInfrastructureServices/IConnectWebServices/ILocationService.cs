using Connect.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>?> GetLocationsAsync(bool forceRefresh = true, CancellationToken token = default);
        Task<IEnumerable<Location>?> GetLocationsCacheAsync();
        IObservable<Location> GetLocation(string locationId, bool forceRefresh = true, CancellationToken token = default);
        Task<bool?> TestNotification(string locationId, CancellationToken token = default);
    }
}
