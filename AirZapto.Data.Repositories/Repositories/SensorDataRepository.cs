using AirZapto.Data.Entities;
using AirZapto.Data.Services.Repositories;
using Framework.Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirZapto.Data.Repositories
{
    public partial class Repository : IRepository
	{
		#region Methods

		public async Task<bool> AddSensorDataAsync(SensorDataEntity entity)
		{
			bool res = false;
			if (this.DataContext != null)
			{
				await this.DataContext.SensorDataEntities.AddAsync(entity);
				res = (await this.DataContext.SaveChangesAsync() > 0) ? true : false;
			}
			return res;
		}

		public async Task<bool> DeleteSensorDataAsync(TimeSpan span)
		{
            bool res = false;
			if (this.DataContext != null)
			{
				var query = await (from s in this.DataContext.SensorDataEntities
								   where ((Clock.Now - span) > s.CreationDateTime)
								   select s).ToListAsync();

				foreach (var entity in query)
				{
                    //Search the entity in the local context 
                    var existingEntity = this.DataContext.SensorDataEntities.Local.FirstOrDefault(e => e.Id == entity.Id);
                    if (existingEntity != null)
                    {
                        //Detach the entity from the context
                        this.DataContext.Entry(existingEntity).State = EntityState.Detached;
                    }

                    //Rattach
                    this.DataContext.Entry(entity).State = EntityState.Unchanged;
                }

                this.DataContext.SensorDataEntities.RemoveRange(query);
				res = (await this.DataContext.SaveChangesAsync() > 0) ? true : false;
			}
			return res;
		}

		public async Task<IEnumerable<SensorDataEntity>?> GetSensorDataAsync(string sensorId, int minutes)
		{
			TimeSpan span = new TimeSpan(0, minutes, 0);
			IEnumerable<SensorDataEntity>? entities = null;

			if (this.DataContext != null)
			{
				entities = await (from s in this.DataContext.SensorDataEntities
								  where ((s.SensorId == sensorId) && (Clock.Now - span) <= s.CreationDateTime)
								  select s).AsNoTracking().ToListAsync();
			}

			return entities;
		}

		public async Task<DateTime?> GetTimeLastSensorData(string sensorId)
		{
			DateTime? timestamp = null;
			if (this.DataContext != null)
			{
				timestamp = await (from s in this.DataContext.SensorDataEntities
							  where (s.SensorId == sensorId)
							  select s.CreationDateTime).MaxAsync();
			}
			return timestamp;
		}

		#endregion
	}
}
