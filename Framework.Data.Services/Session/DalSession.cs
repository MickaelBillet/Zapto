using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.Data.Session
{
    public sealed class DalSession : IDalSession
    {
        #region Properties

        public IDataContextFactory? DataContextFactory
        {
            get; 
        }

        public ConnectionType? ConnectionType 
        { 
            get; 
        }

        #endregion

        #region Constructor
        public DalSession(IServiceProvider serviceProvider, 
                            string connectionStringKey, 
                            string serverTypeKey)
        {
            ISecretService secretService = serviceProvider.GetRequiredService<ISecretService>();
            this.DataContextFactory = serviceProvider.GetRequiredService<IDataContextFactory>();
            this.ConnectionType = ConnectionString.GetConnectionType(secretService, connectionStringKey, serverTypeKey);
        }

        public DalSession(ISecretService secretService,
                            IDataContextFactory dataContextFactory, 
                            string connectionStringKey,
                            string serverTypeKey)
        {
            this.DataContextFactory = dataContextFactory;
            this.ConnectionType = ConnectionString.GetConnectionType(secretService, connectionStringKey, serverTypeKey);
        }

        public DalSession(IServiceProvider serviceProvider)
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            this.DataContextFactory = serviceProvider.GetRequiredService<IDataContextFactory>();
            this.ConnectionType = ConnectionString.GetConnectionType(configuration);
        }
        #endregion

        #region Methods
        public void Dispose()
        {
        }
        #endregion
    }
}
