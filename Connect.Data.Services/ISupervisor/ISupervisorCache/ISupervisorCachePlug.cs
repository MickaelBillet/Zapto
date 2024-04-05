using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorCachePlug : ISupervisorCache
    {
        public Task<ResultCode> AddPlug(Plug plug);
        public Task<IEnumerable<Plug>> GetPlugs();
        public Task<Plug> GetPlug(string id);
        public Task<ResultCode> UpdatePlug(Plug plug);
        public Task<Plug> GetPlug(string address, string unit);
        public Task<ResultCode> ResetWorkingDuration(Plug plug);
    }
}
