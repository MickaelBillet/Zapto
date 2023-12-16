using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorPlug
    {
        Task<ResultCode> PlugExists(string id);
        Task<IEnumerable<Plug>> GetPlugs();
        Task<Plug> GetPlug(string address, string unit);
        Task<Plug> GetPlug(string id);
        Task<ResultCode> AddPlug(Plug plug);
        Task<ResultCode> UpdatePlug(Plug plug);
        Task<ResultCode> ResetWorkingDuration(Plug plug);
        Task<ResultCode> Upgrade1_1();
    }
}
