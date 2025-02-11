using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirZapto.Data.Entities;
using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AirZapto.Data.Repositories
{
    public class SensorRepository : Repository, ISensorRepository
	{
        #region Constructor
        public SensorRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory) { }
        #endregion

        #region Methods
        public async Task<SensorEntity?> GetSensorAsync(string id)
		{
			SensorEntity? sensorEntity = null;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					sensorEntity = await (from sensor in context.Set<SensorEntity>()
										  where sensor.Id == id
										  select sensor).AsNoTracking().FirstOrDefaultAsync();
				}
			});

			return sensorEntity;
		}

		public async Task<SensorEntity?> GetSensorAsync(string channel, string? name)
		{
            SensorEntity? sensorEntity = null;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					sensorEntity = await (from s in context.Set<SensorEntity>()
										  where s.Channel.Equals(channel) && s.Name.Equals(name)
										  select s).AsNoTracking().FirstOrDefaultAsync();
				}
			});
			return sensorEntity;
		}

		public async Task<SensorEntity?> GetSensorFromIdSocketAsync(string idSocket)
		{
            SensorEntity? sensorEntity = null;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					sensorEntity = await (from s in context.Set<SensorEntity>()
										  where s.IdSocket == idSocket
										  select s).AsNoTracking().FirstOrDefaultAsync();
				}
			});
			return sensorEntity;
		}

		public async Task<IEnumerable<SensorEntity>?> GetAllSensorsAsync()
		{
			IEnumerable<SensorEntity>? sensorEntities = null;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					sensorEntities = await (from sensor in context.Set<SensorEntity>()
											select sensor).AsNoTracking().ToListAsync();
				}
			});

			return sensorEntities;
		}

		public bool SensorExists(string sensorId)
		{
            bool res = false;
            this.DataContextFactory.UseContext((context) =>
            {
                res = (context != null) ? (context.Set<SensorEntity>().AsNoTracking().FirstOrDefault((arg) => arg.Id == sensorId) != null) : false;
            });
            return res;
        }

		public async Task<bool> AddSensorAsync(SensorEntity entity)
		{
			bool res = false;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if ((context != null) && (this.SensorExists(entity.Id)) == false)
				{
					await context.Set<SensorEntity>().AddAsync(entity);
					res = (await context.SaveChangesAsync() > 0) ? true : false;
				}
			});

			return res;
		}

		public async Task<bool> DeleteSensorAsync(SensorEntity entity)
		{
			bool res = false;
            await this.DataContextFactory.UseContext(async (context) =>
            {
                if ((context != null) && (this.SensorExists(entity.Id)) == true)
				{
					//Search the entity in the local context 
					var existingEntity = context.Set<SensorEntity>().Local.FirstOrDefault(e => e.Id == entity.Id);
					if (existingEntity != null)
					{
						//Detach the entity from the context
						context.Entry(existingEntity).State = EntityState.Detached;
					}

					//Rattach
					context.Entry(entity).State = EntityState.Unchanged;

					context.Set<SensorEntity>().Remove(entity);
					res = (await context.SaveChangesAsync() > 0) ? true : false;
				}
            });
            return res;
		}

		public async Task<bool> UpdateSensorAsync(SensorEntity entity)
		{
			bool res = false;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					context.DetachLocal<SensorEntity>(entity, entity.Id);
					res = (await context.SaveChangesAsync() > 0) ? true : false;
				}
			});
			return res;
		}
		#endregion
	}
}
