using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
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
            IEnumerable<OperationRange>? operationRanges = await WebService.GetCollectionAsync<OperationRange>(string.Format(ConnectConstants.RestUrlProgramOperationRanges, programId), SerializerOptions, token);
            return operationRanges != null ? new ObservableCollection<OperationRange>(operationRanges) : null;
        }

        public async Task<bool?> DeleteOperationsRanges(string programId, CancellationToken token = default)
        {
            return await WebService.DeleteAsync<OperationRange>(ConnectConstants.RestUrlProgramOperationRanges, programId, token); ;
        }

        /// <summary>
        /// Delete the operation ranges async.
        /// </summary>
        public async Task<bool?> DeleteOperationRangeAsync(OperationRange op, CancellationToken token = default)
        {
            return await WebService.DeleteAsync<OperationRange>(ConnectConstants.RestUrlOperationRangesId, op.Id, token); ;
        }

        public async Task<bool?> AddOperationRangeAsync(OperationRange op, CancellationToken token = default)
        {
            return await WebService.PostAsync<OperationRange>(ConnectConstants.RestUrlOperationRanges, op, token); ;
        }

        #endregion
    }
}
