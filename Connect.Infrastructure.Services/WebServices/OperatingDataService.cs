using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
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
            return await WebService.GetCollectionAsync<OperatingData>(string.Format(ConnectConstants.RestUrlRoomOperatingData, day.Value.ToString("dd-MM-yyyy"), roomId), SerializerOptions, token); ;
        }

        public async Task<DateTime?> GetRoomMaxDate(string roomId, CancellationToken token = default)
        {
            return await WebService.GetAsync<DateTime?>(ConnectConstants.RestUrlMaxDateOperatingData, roomId, SerializerOptions, token); ;
        }

        public async Task<DateTime?> GetRoomMinDate(string roomId, CancellationToken token = default)
        {
            return await WebService.GetAsync<DateTime?>(ConnectConstants.RestUrlMinDateOperatingData, roomId, SerializerOptions, token); ;
        }

        #endregion
    }
}
