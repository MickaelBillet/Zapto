using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class PlugService : ConnectWebService, IPlugService
    {
        #region Property

        #endregion

        #region Constructor

        public PlugService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }

        #endregion

        #region Method

        /// <summary>
        /// Updates the mode async.
        /// </summary>
        public async Task<bool?> ManualProgrammingAsync(Plug plug)
        {
            bool? res = false;

            try
            {
                int[] content = { plug.OnOff, plug.Mode };
                res = await WebService.PutAsync<int[]>(ConnectConstants.RestUrlPlugCommand, content, plug.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// Updates the order async.
        /// </summary>
        public async Task<bool?> OnOffAsync(Plug plug)
        {
            bool? res = false;

            try
            {
                int[] content = { plug.OnOff, plug.Mode };
                res = await WebService.PutAsync<int[]>(ConnectConstants.RestUrlPlugOrder, content, plug.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        #endregion
    }
}
