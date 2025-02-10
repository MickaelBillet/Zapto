using Framework.Infrastructure.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Framework.Data.Services
{
    public class DropDatabaseStartupTask : ICleanTask
    {
        #region Services
        private IDatabaseService DatabaseService { get; }
        #endregion

        #region Constructor
        public DropDatabaseStartupTask(IServiceProvider serviceProvider)
        {
            this.DatabaseService = serviceProvider.GetRequiredService<IDatabaseService>();  
        }
        #endregion

        #region Methods
        public async Task Execute()
        {
            await this.DatabaseService.DropDatabaseAsync();
        }
        #endregion
    }
}
