using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal sealed class ApplicationConnectLocationServices : IApplicationConnectLocationServices
	{
		#region Services

		private ILocationService? LocationService { get; }

		#endregion

		#region Constructor

		public ApplicationConnectLocationServices(IServiceProvider serviceProvider)
		{
			this.LocationService = serviceProvider.GetService<ILocationService>();
		}

		#endregion

		#region Methods
		public async Task<IEnumerable<Location>?> GetLocationCache()
		{
			return (this.LocationService != null) ?  await this.LocationService.GetLocationsCacheAsync() : null;
		}

		public async Task<IEnumerable<Location>?> GetLocations()
		{
			return (this.LocationService != null) ? await this.LocationService.GetLocationsAsync(true) : null;
		}

		public IObservable<Location>? GetLocation(string id)
		{
			return this.LocationService?.GetLocation(id, true);
		}

		public async Task<bool?> TestNotication(string locationId)
		{
			return (this.LocationService != null) ? await this.LocationService.TestNotification(locationId) : null;
		}
		#endregion
	}
}
