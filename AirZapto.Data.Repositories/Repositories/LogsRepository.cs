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
        public LogsRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory) { }
        #endregion

        #region Methods
        public LogsEntity? GetLogs(string id)
		{
			LogsEntity? entity = null;
			this.DataContextFactory.UseContext((context) =>
			{
				if (context != null)
                {
                    entity = (from logs in context.Set<LogsEntity>()
                              where logs.Id == id
                              select logs).AsNoTracking().FirstOrDefault();
                }
			});
			return entity;
		}

		public async Task<IEnumerable<LogsEntity>?> GetAllLogsAsync()
		{
			IEnumerable<LogsEntity>? entities = null;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					entities = await (from logs in context.Set<LogsEntity>()
                                      select logs).AsNoTracking().ToListAsync();
				}
			});
			return entities;
		}

		public bool LogsExists(string logId)
		{
			bool res = false;
            this.DataContextFactory.UseContext((context) =>
            {
                res = (context != null) ? (context.Set<LogsEntity>().AsNoTracking().FirstOrDefault((arg) => arg.Id == logId) != null) : false;
            });
			return res;
        }

		public bool AddLogs(LogsEntity entity)
		{
			bool res = false;
			this.DataContextFactory.UseContext((context) =>
			{
				if (context != null)
				{
					context.Set<LogsEntity>().Add(entity);
					res = (context.SaveChanges() > 0) ? true : false;
				}
			});
			return res;
		}

		public async Task<bool> DeleteLogsAsync(LogsEntity entity)
		{
			bool res = false;
            await this.DataContextFactory.UseContext(async (context) =>
			{
				if ((context != null) && (this.LogsExists(entity.Id) == true))
				{
					//Search the entity in the local context 
					var existingEntity = context.Set<LogsEntity>().Local.FirstOrDefault(e => e.Id == entity.Id);
					if (existingEntity != null)
					{
						//Detach the entity from the context
						context.Set<LogsEntity>().Entry(existingEntity).State = EntityState.Detached;
					}

                    //Rattach
                    context.Set<LogsEntity>().Entry(entity).State = EntityState.Unchanged;
					context.Set<LogsEntity>().Remove(entity);
					res = (await context.SaveChangesAsync() > 0) ? true : false;
				}
			});

            return res;
		}

		public async Task<bool> UpdateLogsAsync(LogsEntity entity)
		{
			bool res = false;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if ((context != null) && (this.LogsExists(entity.Id) == true))
				{
					context.DetachLocal<LogsEntity>(entity, entity.Id);
					res = (await context.SaveChangesAsync() > 0) ? true : false;
				}
			});
			return res;
		}
		#endregion
	}
}
