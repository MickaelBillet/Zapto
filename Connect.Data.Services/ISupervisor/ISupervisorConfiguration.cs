using Connect.Model;
using Framework.Core.Base;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorConfiguration
    {
        Task<ResultCode> AddConfiguration(Configuration configuration);
    }
}
