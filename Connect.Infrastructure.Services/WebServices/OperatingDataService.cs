﻿using Connect.Application.Infrastructure;
using Connect.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class OperatingDataService : ConnectWebService, IOperatingDataService
    {
        #region Constructor

        public OperatingDataService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        {
        }

        #endregion

        #region Method   
        public async Task<IEnumerable<OperatingData>?> GetRoomOperatingDataOfDay(string roomId, DateTime? day, CancellationToken token = default)
        {
            return await WebService.GetCollectionAsync<OperatingData>(string.Format(ConnectConstants.RestUrlRoomOperatingData, day!.Value.ToString("yyyy-MM-ddTHH:mm:ss"), roomId), SerializerOptions, token); ;
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
