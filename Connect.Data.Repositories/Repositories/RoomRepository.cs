using Connect.Data.Entities;
using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Connect.Data.Repositories
{
    public class RoomRepository : Repository<RoomEntity>, IRoomRepository
    {
        #region Constructor
        public RoomRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory) 
        { }
        #endregion

        #region Methods
        public async Task<RoomEntity?> GetFromPlugIdAsync(string plugId)
        {
            RoomEntity? entity = null;
            await this.DataContextFactory.UseContext(async (context) =>
            {
                DbSet<RoomEntity>? table = context?.Set<RoomEntity>();
                if (table != null)
                {
                    entity = await table.FromSql<RoomEntity>($"SELECT * FROM room INNER JOIN connectedObject ON connectedObject.RoomId = room.Id INNER JOIN plug ON connectedobject.Id == plug.ConnectedObjectId WHERE plug.Id = {plugId}").AsNoTracking().FirstOrDefaultAsync<RoomEntity>();
                }
            });
            return entity;
        }
        #endregion
    }
}
