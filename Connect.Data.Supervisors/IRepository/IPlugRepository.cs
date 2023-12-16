using Connect.Data.Entities;
using Framework.Data.Abstractions;

namespace Connect.Data.Services.Repositories
{
    public interface IPlugRepository : IRepository<PlugEntity>
    {
        Task<int> Upgrade1_1();
    }
}
