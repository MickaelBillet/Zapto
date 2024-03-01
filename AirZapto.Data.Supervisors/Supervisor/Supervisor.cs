using AirZapto.Data.Services.Repositories;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace AirZapto.Data.Supervisors
{
    public abstract class Supervisor
	{
        private readonly Lazy<IRepository>? _lazyRepository;

		#region Properties
		public ConnectionType? ConnectionType { get; set; }
        protected IRepository? Repository => _lazyRepository?.Value;
		#endregion

		#region Constructor
		public Supervisor(IServiceProvider serviceProvider) 
		{
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            IDataContextFactory contextFactory = serviceProvider.GetRequiredService<IDataContextFactory>();
            IRepositoryFactory repositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();

            ConnectionType connectionType = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = (configuration["ConnectionStrings:ServerType"] == "Sqlite") ? ServerType.SqlLite : ServerType.MySql,
            };

            (IDbConnection? connection, IDataContext? context)? obj = contextFactory.CreateDbContext(connectionType.ConnectionString, connectionType.ServerType);
            if ((obj != null) && (obj.Value.context != null))
            {
                _lazyRepository = repositoryFactory?.CreateRepository(obj.Value.context);
            }
        }

        public Supervisor(IDataContextFactory contextFactory, IRepositoryFactory repositoryFactory, IConfiguration? configuration)
        {
            if ((contextFactory != null) && (configuration != null))
            {
                ConnectionType connectionType = new ConnectionType()
                {
                    ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                    ServerType = (configuration["ConnectionStrings:ServerType"] == "Sqlite") ? ServerType.SqlLite : ServerType.MySql,
                };

                (IDbConnection? connection, IDataContext? context)? obj = contextFactory.CreateDbContext(connectionType.ConnectionString, connectionType.ServerType);
                if ((obj != null) && (obj.Value.context != null))
                {
                    _lazyRepository = repositoryFactory?.CreateRepository(obj.Value.context);
                }
            }
        }
        #endregion
    }
}
