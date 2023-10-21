using Connect.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Application
{
	public interface IApplicationLocationServices
	{
		Task<IEnumerable<Location>?> GetLocationCache();
		Task<IEnumerable<Location>?> GetLocations();
		IObservable<Location>? GetLocation(string id);
		Task<bool?> TestNotication(string locationId);
    }
}
