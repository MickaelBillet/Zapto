using AirZapto.Data.Entities;
using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirZapto.Data.Repositories
{
    public class LogsRepository : Repository, ILogsRepository
	{
        #region Constructor
        public LogsRepository(IDalSession session) : base(session)
        { }
        #endregion

        #region Methods
        public LogsEntity? GetLogs(string id)
		{
			LogsEntity? entity = null;

			if (this.DataContext != null)
			{
                entity = (from logs in this.DataContext.LogsEntities
								   where logs.Id == id
								   select logs).AsNoTracking().FirstOrDefault();
			}

			return entity;
		}

		public async Task<IEnumerable<LogsEntity>?> GetAllLogsAsync()
		{
			IEnumerable<LogsEntity>? entities = null;
			if (this.DataContext != null)
			{
                entities = await (from logs in this.DataContext.LogsEntities
								   select logs).AsNoTracking().ToListAsync();
			}

			return entities;
		}

		public bool LogsExists(string sensorId)
		{
			return (this.DataContext != null) ? (this.DataContext.LogsEntities.AsNoTracking().FirstOrDefault((arg) => arg.Id == sensorId) != null) : false;
		}

		public bool AddLogs(LogsEntity entity)
		{
			bool res = false;

			if (this.DataContext != null)
			{
				this.DataContext.LogsEntities.Add(entity);
				res = (this.DataContext.SaveChanges() > 0) ? true : false;
			}

			return res;
		}

		public async Task<bool> DeleteLogsAsync(LogsEntity entity)
		{
			bool res = false;

			if ((this.DataContext != null) && (this.LogsExists(entity.Id) == true))
			{
                //Search the entity in the local context 
                var existingEntity = this.DataContext.LogsEntities.Local.FirstOrDefault(e => e.Id == entity.Id);
                if (existingEntity != null)
                {
                    //Detach the entity from the context
                    this.DataContext.Entry(existingEntity).State = EntityState.Detached;
                }

                //Rattach
                this.DataContext.Entry(entity).State = EntityState.Unchanged;

                this.DataContext.Remove<LogsEntity>(entity);
				res = (await this.DataContext.SaveChangesAsync() > 0) ? true : false;
			}

			return res;
		}

		public async Task<bool> UpdateLogsAsync(LogsEntity entity)
		{
			bool res = false;

			if (this.DataContext != null)
			{
                this.DataContext.DetachLocal<LogsEntity>(entity, entity.Id);
                res = (await this.DataContext.SaveChangesAsync() > 0) ? true : false;
			}

			return res;
		}

		#endregion
	}
}
