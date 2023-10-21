using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public abstract class ScopedService : BackgroundService
    {
        private IServiceScopeFactory ServiceScopeFactory {get;}
        protected ScopedService(IServiceScopeFactory serviceScopeFactory) : base()
        {
            this.ServiceScopeFactory = serviceScopeFactory;
        }

        //Create and launch the scoped service
        protected async Task Process()
        {
            //Create by factory
            using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
            {
                //Process in a task
                await ProcessInScope(scope);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }

        public abstract Task ProcessInScope(IServiceScope scope);
    }
}
