using Connect.Data.DataContext;
using Connect.Data.Entities;
using Connect.Data.Repository;
using Framework.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Test.Data.Supervisor
{
    public abstract class SupervisorTest
    {
        protected IServiceProvider ServiceProvider { get; }
        protected DbContextOptions ContextOptions { get; }

        public SupervisorTest() 
        { 
            ServiceCollection services = new ServiceCollection();
            services.AddRepositories();
            this.ServiceProvider = services.BuildServiceProvider();

            this.ContextOptions = new DbContextOptionsBuilder()
                                                .UseInMemoryDatabase("ConnectDatabase")
                                                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                                                .Options;

            this.ResetDatabase();
        }

        protected void ResetDatabase()
        {
            using (ConnectContextInMemory context = new ConnectContextInMemory(this.ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.AddRange(
                    new LogsEntity { Id = "0", CreationDateTime = Clock.Now, Level = "0", Properties = "Toto" }); ;

                context.SaveChanges();
            }
        }

        public ConnectContextInMemory CreateContext() => new ConnectContextInMemory(this.ContextOptions);
    }
}
