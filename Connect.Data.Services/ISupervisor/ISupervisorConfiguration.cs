using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorConfiguration
    {
        Task<ResultCode> AddConfiguration(Configuration configuration);
        Task<IEnumerable<Configuration>> GetConfigurations();
    }
}
