using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Framework.Data.Services
{
    public class CreateDatabaseStartupTask : IStartupTask
    {
        #region Services
        private IDatabaseService DatabaseService { get; }
        #endregion

        #region Constructor
        public CreateDatabaseStartupTask(IServiceProvider serviceProvider)
        {
            this.DatabaseService = serviceProvider.GetRequiredService<IDatabaseService>();  
        }
        #endregion

        #region Methods
        public async Task Execute()
        {
            await this.DatabaseService.ConfigureDatabase();
        }
        #endregion
    }
}
