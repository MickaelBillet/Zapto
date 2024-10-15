using Connect.Model;
using Framework.Core.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorConfiguration : ISupervisor
    {
        Task<ResultCode> AddConfiguration(Configuration configuration);
        Task<IEnumerable<Configuration>> GetConfigurations();
    }
}
