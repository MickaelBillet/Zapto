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

        public IDataContext? DataContext
        {
            get; private set;
        }

        public ConnectionType? ConnectionType 
        { 
            get; private set; 
        }

        #endregion

        #region Constructor
        public DalSession(IServiceProvider serviceProvider, 
                            string connectionStringKey, 
                            string serverTypeKey)
        {
            ISecretService secretService = serviceProvider.GetRequiredService<ISecretService>();
            IDataContextFactory dataContextFactory = serviceProvider.GetRequiredService<IDataContextFactory>();
            this.ConnectionType = ConnectionString.GetConnectionType(secretService, connectionStringKey, serverTypeKey);
            if (string.IsNullOrEmpty(this.ConnectionType?.ConnectionString) == false)
            {
                dataContextFactory.UseContext(context =>
                {
                    if (context != null)
                    {
                        this.DataContext = context;
                    }
                });
            }
        }

        public DalSession(ISecretService secretService,
                            IDataContextFactory dataContextFactory, 
                            string connectionStringKey,
                            string serverTypeKey)
        {
            this.ConnectionType = ConnectionString.GetConnectionType(secretService, connectionStringKey, serverTypeKey);
            if (string.IsNullOrEmpty(this.ConnectionType?.ConnectionString) == false)
            {
                dataContextFactory.UseContext(context =>
                {
                    if (context != null)
                    {
                        this.DataContext = context;
                    }
                });
            }
        }

        public DalSession(IServiceProvider serviceProvider)
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            IDataContextFactory dataContextFactory = serviceProvider.GetRequiredService<IDataContextFactory>();
            this.ConnectionType = ConnectionString.GetConnectionType(configuration);
            if (string.IsNullOrEmpty(this.ConnectionType?.ConnectionString) == false)
            {
                dataContextFactory.UseContext(context =>
                {
                    if (context != null)
                    {
                        this.DataContext = context;
                    }
                });
            }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            this.DataContext?.Dispose();
        }
        #endregion
    }
}
