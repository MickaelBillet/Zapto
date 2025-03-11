using Connect.Data.Entities;
using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Framework.Data.Repository;
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
                    entity = await table.FromSqlInterpolated($@"SELECT r.* 
                                                                FROM room r
                                                                INNER JOIN connectedObject co ON co.RoomId = r.Id 
                                                                INNER JOIN plug p ON co.Id = p.ConnectedObjectId 
                                                                WHERE p.Id = {plugId}")
                                                            .AsNoTracking()
                                                            .FirstOrDefaultAsync();

                }
            });
            return entity;
        }
        #endregion
    }
}
