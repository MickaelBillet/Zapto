using Connect.Data.Entities;
using Framework.Core.Data;
using Framework.Data.Abstractions;
using System.Threading.Tasks;

namespace Connect.Data.Services.Repositories
{
    public interface IRoomRepository : IRepository<RoomEntity>
    {
        Task<RoomEntity?> GetFromPlugIdAsync(string plugId);
    }
}
