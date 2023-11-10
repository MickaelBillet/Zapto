using Connect.Data.Entities;
using Framework.Data.Abstractions;
using System.Threading.Tasks;

namespace Connect.Data.Services.Repositories
{
    public interface IServerIotStatusRepository : IRepository<ServerIotStatusEntity>
    {
        Task<int> CreateTable();
    }
}
