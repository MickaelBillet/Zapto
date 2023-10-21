using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class OperationRangeService : ConnectWebService, IOperationRangeService
    {
        #region Property

        #endregion

        #region Constructor

        public OperationRangeService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }

        #endregion

        #region Method    

        public async Task<ObservableCollection<OperationRange>?> GetOperationsRanges(string programId, CancellationToken token = default)
        {
            IEnumerable<OperationRange>? operationRanges = null;

            try
            {
                operationRanges = await WebService.GetCollectionAsync<OperationRange>(string.Format(ConnectConstants.RestUrlProgramOperationRanges, programId), SerializerOptions, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return operationRanges != null ? new ObservableCollection<OperationRange>(operationRanges) : null;
        }

        public async Task<bool?> DeleteOperationsRanges(string programId, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.DeleteAsync<OperationRange>(ConnectConstants.RestUrlProgramOperationRanges, programId, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// Delete the operation ranges async.
        /// </summary>
        public async Task<bool?> DeleteOperationRangeAsync(OperationRange op, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.DeleteAsync<OperationRange>(ConnectConstants.RestUrlOperationRangesId, op.Id, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<bool?> AddOperationRangeAsync(OperationRange op, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.PostAsync<OperationRange>(ConnectConstants.RestUrlOperationRanges, op, token);
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
