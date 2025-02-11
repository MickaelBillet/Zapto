using AirZapto.Data.Services.Repositories;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using System;

namespace AirZapto.Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public Lazy<ISensorRepository>? CreateSensorRepository(IDalSession session)
        {
            Lazy<ISensorRepository>? repository = null;
            if (session.DataContextFactory != null)
            {
                if (session.ConnectionType?.ServerType == ServerType.SqlLite)
                {
                    repository = new Lazy<ISensorRepository>(() => new SensorRepository(session.DataContextFactory));
                }
            }
            return repository;
        }

        public Lazy<ISensorDataRepository>? CreateSensorDataRepository(IDalSession session)
        {
            Lazy<ISensorDataRepository>? repository = null;
            if (session.DataContextFactory != null)
            {
                if (session.ConnectionType?.ServerType == ServerType.SqlLite)
                {
                    repository = new Lazy<ISensorDataRepository>(() => new SensorDataRepository(session.DataContextFactory));
                }
            }
            return repository;
        }

        public Lazy<IVersionRepository>? CreateVersionRepository(IDalSession session)
        {
            Lazy<IVersionRepository>? repository = null;
            if (session.DataContextFactory != null)
            {
                if (session.ConnectionType?.ServerType == ServerType.SqlLite)
                {
                    repository = new Lazy<IVersionRepository>(() => new VersionRepository(session.DataContextFactory));
                }
            }
            return repository;
        }

        public Lazy<ILogsRepository>? CreateLogsRepository(IDalSession session)
        {
            Lazy<ILogsRepository>? repository = null;
            if (session.DataContextFactory != null)
            {
                if (session.ConnectionType?.ServerType == ServerType.SqlLite)
                {
                    repository = new Lazy<ILogsRepository>(() => new LogsRepository(session.DataContextFactory));
                }
            }
            return repository;
        }
    }
}
 