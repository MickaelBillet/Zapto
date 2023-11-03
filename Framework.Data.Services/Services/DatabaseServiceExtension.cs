using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Framework.Data.Services
{
    public static class DatabaseServiceExtension
    {
        public static void ConfigureDatabase(this IApplicationBuilder app, IConfiguration configuration) 
        {
            using (IDatabaseService service = app.ApplicationServices.GetRequiredService<IDatabaseService>())
            {
                ConnectionType connectionType = new ConnectionType()
                {
                    ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                    ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"])
                };

                bool isCreated = false;
                if (service != null)
                {
                    if (service.DatabaseExist(connectionType) == false)
                    {
                        isCreated = service.CreateDatabase(connectionType);

                        if (isCreated == true)
                        {
                            Task.Run(async () => await service.FeedDataAsync());
                        }
                    }

                    if (service.DatabaseExist(connectionType) == true)
                    {
                        Task.Run(async () =>
                        {
                            bool isUpgraded = await service.UpgradeDatabaseAsync();
                            await service.InitializeDataAsync();
                        });
                    }
                }
            }
        }
    }
}
