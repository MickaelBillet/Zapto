using Connect.Data;
using Connect.Data.DataContext;
using Connect.Data.Repositories;
using Connect.Data.Supervisors;
using Connect.Model;
using Framework.Common.Services;
using Framework.Core.Domain;
using Framework.Data.Session;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Connect.WebServer.Services
{
    public class DatabaseSinkService : ILogEventSink
    {
        #region Properties
        private LogEventLevel Level { get; } = LogEventLevel.Error;
        private ISupervisorLog? Supervisor { get; }
        #endregion

        #region Constructor
        public DatabaseSinkService(IConfiguration configuration)
        {
            if (configuration != null)
            {
                this.Level = SeverityToLevel(configuration["Serilog.MinimumLevel"]);
                ISecretService? secretService = SecretService.GetSecretService(configuration);

                this.Supervisor = new SupervisorLog(new DalSession(secretService!,
                                                                    new DataContextFactory(secretService!, ConnectConstants.ConnectionStringConnectKey, ConnectConstants.ServerTypeConnectKey),
                                                                    ConnectConstants.ConnectionStringConnectKey,
                                                                    ConnectConstants.ServerTypeConnectKey),
                                                    new RepositoryFactory());
            }            
        }
        #endregion

        #region Methods
        public void Emit(LogEvent logEvent)
        {
            if ((this.Supervisor != null) && (logEvent.Level >= this.Level))
            {
                //Why I don't have the warning CS4014 
                this.Supervisor.AddLog(new Logs()
                {
                    Date = logEvent.Timestamp.DateTime,
                    Level = LevelToSeverity(logEvent),
                    Exception = logEvent.Exception?.Message,
                    RenderedMessage = logEvent.RenderMessage(),
                });
            }
        }

        static string LevelToSeverity(LogEvent logEvent)
        {
            return logEvent.Level switch
            {
                LogEventLevel.Debug => "Debug",
                LogEventLevel.Error => "Error",
                LogEventLevel.Fatal => "Fatal",
                LogEventLevel.Verbose => "Verbose",
                LogEventLevel.Warning => "Warning",
                _ => "Information",
            };
        }

        static LogEventLevel SeverityToLevel(string? severity)
        {
            return severity switch
            {
                "Debug" => LogEventLevel.Debug,
                "Error" => LogEventLevel.Error,
                "Fatal" => LogEventLevel.Fatal,
                "Verbose" => LogEventLevel.Verbose,
                "Warning" => LogEventLevel.Warning,
                _ => LogEventLevel.Information,
            };
        }
        #endregion
    }
}
