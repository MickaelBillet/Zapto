using Connect.Data.Entities;
using Framework.Data.Abstractions;

namespace Connect.Data.Services.Repositories
{
    public interface IRoomRepository : IRepository<RoomEntity>
    {
        Task<RoomEntity> GetFromPlugIdAsync(string plugId);
    }
}
