using AirZapto.Data.Entities;
using AirZapto.Data.Services.Repositories;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirZapto.Data.Repositories
{
    public class SensorDataRepository : Repository, ISensorDataRepository
	{
        #region Constructor
        public SensorDataRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory) { }
        #endregion

        #region Methods
        public async Task<bool> AddSensorDataAsync(SensorDataEntity entity)
		{
			bool res = false;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					await context.Set<SensorDataEntity>().AddAsync(entity);
					res = (await context.SaveChangesAsync() > 0) ? true : false;
				}
			});
			return res;
		}

		public async Task<bool> DeleteSensorDataAsync(TimeSpan span)
		{
            bool res = false;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					var query = await (from s in context.Set<SensorDataEntity>()
									   where ((Clock.Now - span) > s.CreationDateTime)
									   select s).ToListAsync();

					foreach (var entity in query)
					{
						//Search the entity in the local context 
						var existingEntity = context.Set<SensorDataEntity>().Local.FirstOrDefault(e => e.Id == entity.Id);
						if (existingEntity != null)
						{
							//Detach the entity from the context
							context.Entry(existingEntity).State = EntityState.Detached;
						}

						//Rattach
						context.Entry(entity).State = EntityState.Unchanged;
					}

					context.Set<SensorDataEntity>().RemoveRange(query);
					res = (await context.SaveChangesAsync() > 0) ? true : false;
				}
			});
			return res;
		}

		public async Task<IEnumerable<SensorDataEntity>?> GetSensorDataAsync(string sensorId, int minutes)
		{
			TimeSpan span = new TimeSpan(0, minutes, 0);
			IEnumerable<SensorDataEntity>? entities = null;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					entities = await (from s in context.Set<SensorDataEntity>()
									  where ((s.SensorId == sensorId) && (Clock.Now - span) <= s.CreationDateTime)
									  select s).AsNoTracking().ToListAsync();
				}
			});
			return entities;
		}

		public async Task<DateTime?> GetTimeLastSensorData(string sensorId)
		{
			DateTime? timestamp = null;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					timestamp = await (from s in context.Set<SensorDataEntity>()
                                       where (s.SensorId == sensorId)
									   select s.CreationDateTime).MaxAsync();
				}
			});
			return timestamp;
		}

		#endregion
	}
}
