using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class OperatingDataService : ConnectWebService, IOperatingDataService
    {
        #region Constructor

        public OperatingDataService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }

        #endregion

        #region Method   
        public async Task<IEnumerable<OperatingData>?> GetRoomOperatingDataOfDay(string roomId, DateTime? day, CancellationToken token = default)
        {
            IEnumerable<OperatingData>? operatingData = null;

            try
            {
                if (day != null)
                {
                    operatingData = await WebService.GetCollectionAsync<OperatingData>(string.Format(ConnectConstants.RestUrlRoomOperatingData, day.Value.ToString("dd-MM-yyyy"), roomId), SerializerOptions, token);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return operatingData;
        }

        public async Task<DateTime?> GetRoomMaxDate(string roomId, CancellationToken token = default)
        {
            DateTime? maxDate = null;

            try
            {
                maxDate = await WebService.GetAsync<DateTime?>(ConnectConstants.RestUrlMaxDateOperatingData, roomId, SerializerOptions, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return maxDate;
        }

        public async Task<DateTime?> GetRoomMinDate(string roomId, CancellationToken token = default)
        {
            DateTime? minDate = null;

            try
            {
                minDate = await WebService.GetAsync<DateTime?>(ConnectConstants.RestUrlMinDateOperatingData, roomId, SerializerOptions, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return minDate;
        }

        #endregion
    }
}
