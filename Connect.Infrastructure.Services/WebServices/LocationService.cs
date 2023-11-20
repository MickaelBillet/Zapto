using Connect.Application.Infrastructure;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class LocationService : ConnectWebService, ILocationService
    {
        #region Property
        private ICacheService? CacheService { get; } ///Cache Client
        #endregion

        #region Constructor

        public LocationService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
            this.CacheService = serviceProvider.GetService<ICacheService>();
        }

        #endregion

        #region Method

        public async Task<IEnumerable<Location>?> GetLocationsAsync(bool forceRefresh = true, CancellationToken token = default)
        {
            IEnumerable<Location>? locations = null;
            if (forceRefresh)
            {
                //Call the webservice
                locations = await WebService.GetCollectionAsync<Location>(ConnectConstants.RestUrlLocations, SerializerOptions, token);
                if (locations != null && this.CacheService != null)
                {
                    //Save the rooms in database
                    foreach (Location location in locations)
                    {
                        //Insert the objets in to the database
                        await this.CacheService.InsertObject<Location>("locations", location);
                    }
                }
            }
            return locations;
        }

        public async Task<IEnumerable<Location>?> GetLocationsCacheAsync()
        {
            return this.CacheService != null ? await this.CacheService.GetAllObjects<Location>() : null; ;
        }

        public async Task<bool?> TestNotification(string locationId, CancellationToken token = default)
        {
            return await WebService.PostAsync<string>(ConnectConstants.RestUrlLocationTest, locationId, token); ;
        }

        public IObservable<Location> GetLocation(string locationId, bool forceRefresh = true, CancellationToken token = default)
        {
            return Observable.Create<Location>(async (observer) =>
            {
                try
                {
                    Location? location = null;

                    try
                    {
                        location = this.CacheService != null ? await this.CacheService.GetObject<Location>("location" + locationId) : null;
                    }
                    finally
                    {
                        if (location != null)
                        {
                            observer.OnNext(location);
                        }

                        if (forceRefresh)
                        {
                            //Call the webservice
                            location = await WebService.GetAsync<Location>(ConnectConstants.RestUrlLocationsId, locationId, SerializerOptions, token);

                            if (location != null)
                            {
                                if (this.CacheService != null)
                                {
                                    //Insert the objets in to the database
                                    await this.CacheService.InsertObject<Location>("location" + location.Id, location);
                                }

                                observer.OnNext(location);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }

                observer.OnCompleted();
            });
        }

        #endregion
    }
}
