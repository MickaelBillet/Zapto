using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorConfiguration : ISupervisor
    {
        Task<ResultCode> AddConfiguration(Configuration configuration);
        Task<IEnumerable<Configuration>> GetConfigurations();
    }
}
