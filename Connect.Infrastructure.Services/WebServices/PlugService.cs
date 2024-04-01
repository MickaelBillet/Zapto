using Connect.Application.Infrastructure;
using Connect.Model;
using System;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class PlugService : ConnectWebService, IPlugService
    {
        #region Property

        #endregion

        #region Constructor

        public PlugService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        {
        }

        #endregion

        #region Method

        /// <summary>
        /// Updates the mode async.
        /// </summary>
        public async Task<bool?> ManualProgrammingAsync(Plug plug)
        {
            return await WebService.PutAsync<int[]>(ConnectConstants.RestUrlPlugCommand, new int[] { plug.OnOff, plug.Mode }, plug.Id); ;
        }

        /// <summary>
        /// Updates the order async.
        /// </summary>
        public async Task<bool?> OnOffAsync(Plug plug)
        {
            return await WebService.PutAsync<int[]>(ConnectConstants.RestUrlPlugOrder, new int[] { plug.OnOff, plug.Mode }, plug.Id); ;
        }

        #endregion
    }
}
