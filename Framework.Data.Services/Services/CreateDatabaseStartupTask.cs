using Framework.Infrastructure.Abstractions;
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
        private int Major { get; }
        private int Minor { get; }
        private int Build { get; }
        #endregion

        #region Constructor
        public CreateDatabaseStartupTask(IServiceProvider serviceProvider, int major, int minor, int build)
        {
            this.DatabaseService = serviceProvider.GetRequiredService<IDatabaseService>();  
            this.Major = major;
            this.Minor = minor;
            this.Build = build;
        }
        #endregion

        #region Methods
        public async Task Execute()
        {
            await this.DatabaseService.ConfigureDatabase(this.Major, this.Minor, this.Build);
        }
        #endregion
    }
}
