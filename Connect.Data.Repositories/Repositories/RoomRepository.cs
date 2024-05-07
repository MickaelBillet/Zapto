using Connect.Data.Entities;
using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Connect.Data.Repositories
{
    public class RoomRepository : Repository<RoomEntity>, IRoomRepository
    {
        #region Constructor
        public RoomRepository(IDalSession session) : base(session) 
        { }
        #endregion

        #region Methods
        public async Task<RoomEntity?> GetFromPlugIdAsync(string plugId)
        {
            RoomEntity? entity = null;
            if (this.DataContext != null)
            {
                entity = await this.DataContext.RoomEntities.FromSql<RoomEntity>($"SELECT * FROM room INNER JOIN connectedObject ON connectedObject.RoomId = room.Id INNER JOIN plug ON connectedobject.Id == plug.ConnectedObjectId WHERE plug.Id = {plugId}").AsNoTracking().FirstOrDefaultAsync<RoomEntity>();
            }
            return entity;
        }
        #endregion
    }
}
