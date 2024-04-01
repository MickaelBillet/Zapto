using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirZapto.Data.Entities;
using AirZapto.Data.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AirZapto.Data.Repositories
{
    public partial class Repository : IRepository
	{
		#region Methods


		public async Task<SensorEntity?> GetSensorAsync(string id)
		{
			SensorEntity? sensorEntity = null;
			if (this.DataContext != null)
			{
                sensorEntity = await (from sensor in this.DataContext.SensorEntities
								   where sensor.Id == id
								   select sensor).AsNoTracking().FirstOrDefaultAsync();
			}

			return sensorEntity;
		}

		public async Task<SensorEntity?> GetSensorAsync(string channel, string? name)
		{
            SensorEntity? sensorEntity = null;
			if (this.DataContext != null)
			{
				sensorEntity = await (from s in this.DataContext.SensorEntities
											  where s.Channel.Equals(channel) && s.Name.Equals(name)
											  select s).AsNoTracking().FirstOrDefaultAsync();
			}

			return sensorEntity;
		}

		public async Task<SensorEntity?> GetSensorFromIdSocketAsync(string idSocket)
		{
            SensorEntity? sensorEntity = null;
			if (this.DataContext != null)
			{
				sensorEntity = await (from s in this.DataContext.SensorEntities
											 where s.IdSocket == idSocket
											 select s).AsNoTracking().FirstOrDefaultAsync();
			}

			return sensorEntity;
		}

		public async Task<IEnumerable<SensorEntity>?> GetAllSensorsAsync()
		{
			IEnumerable<SensorEntity>? sensorEntities = null;
			if (this.DataContext != null)
			{
                sensorEntities = await (from sensor in this.DataContext.SensorEntities
								   select sensor).AsNoTracking().ToListAsync();
			}

			return sensorEntities;
		}

		public async Task<bool> SensorExists(string sensorId)
		{
			return (this.DataContext != null) ? (await this.DataContext.SensorEntities.AsNoTracking().FirstOrDefaultAsync((arg) => arg.Id == sensorId) != null) : false;
		}

		public async Task<bool> AddSensorAsync(SensorEntity entity)
		{
			bool res = false;

			if ((this.DataContext != null) && (await this.SensorExists(entity.Id)) == false)
			{
				await this.DataContext.SensorEntities.AddAsync(entity);
				res = (await this.DataContext.SaveChangesAsync() > 0) ? true : false;
			}

			return res;
		}

		public async Task<bool> DeleteSensorAsync(SensorEntity entity)
		{
			bool res = false;

            if ((this.DataContext != null) && (await this.SensorExists(entity.Id)) == true)
            {
				//Search the entity in the local context 
                var existingEntity = this.DataContext.SensorEntities.Local.FirstOrDefault(e => e.Id == entity.Id);
                if (existingEntity != null)
                {
                    //Detach the entity from the context
                    this.DataContext.Entry(existingEntity).State = EntityState.Detached;
                }

                //Rattach
                this.DataContext.Entry(entity).State = EntityState.Unchanged;

                this.DataContext.Remove<SensorEntity>(entity);
				res = (await this.DataContext.SaveChangesAsync() > 0) ? true : false;
			}

			return res;
		}

		public async Task<bool> UpdateSensorAsync(SensorEntity entity)
		{
			bool res = false;

			if (this.DataContext != null)
			{
                this.DataContext.DetachLocal<SensorEntity>(entity, entity.Id);
                res = (await this.DataContext.SaveChangesAsync() > 0) ? true : false;
			}

			return res;
		}

		#endregion
	}
}
