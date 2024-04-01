using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal class ApplicationOperationDataService : IApplicationOperationDataService
    {
        #region Services
        private IOperatingDataService? OperatingDataService { get; }
        #endregion

        #region Constructor
        public ApplicationOperationDataService(IServiceProvider serviceProvider)
        {
            this.OperatingDataService = serviceProvider.GetRequiredService<IOperatingDataService>();
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<OperatingData>> GetRoomOperatingDataOfDay(string roomId, DateTime? dateTime, CancellationToken token = default)
        {
            IEnumerable<OperatingData>? operatingData = (this.OperatingDataService != null) ? await this.OperatingDataService.GetRoomOperatingDataOfDay(roomId, dateTime, token) : null;
            return (operatingData ?? Enumerable.Empty<OperatingData>());
        }

        public async Task<DateTime?> GetRoomMaxDate(string roomId, CancellationToken token = default)
        {
            return (this.OperatingDataService != null) ? await this.OperatingDataService.GetRoomMaxDate(roomId, token) : null;
        }

        public async Task<DateTime?> GetRoomMinDate(string roomId, CancellationToken token = default)
        {
            return (this.OperatingDataService != null) ? await this.OperatingDataService.GetRoomMinDate(roomId, token) : null;
        }
        #endregion
    }
}
