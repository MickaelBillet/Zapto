using AirZapto.Data;
using AirZapto.Data.DataContext;
using AirZapto.Data.Repositories;
using AirZapto.Data.Services;
using AirZapto.Data.Supervisors;
using Framework.Common.Services;
using Framework.Core.Domain;
using Framework.Data.Session;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace AirZapto.WebServer.Services
{
    public class DatabaseSinkService : ILogEventSink
    {
        #region Properties
        private LogEventLevel Level { get; } = LogEventLevel.Error;
        private IConfiguration? Configuration { get; }
        #endregion

        #region Constructor
        public DatabaseSinkService(IConfiguration configuration)
        {
            if (configuration != null)
            {
                this.Configuration = configuration;
                this.Level = SeverityToLevel(configuration["Serilog.MinimumLevel"]);
            }
        }
        #endregion

        #region Methods
        public void Emit(LogEvent logEvent)
        {
            if ((this.Configuration != null) && (logEvent.Level >= this.Level))
            {
                ISecretService? secretService = null;

                if (byte.Parse(this.Configuration["Secret"]!) == 1)
                {
                    secretService = new VarEnvService();
                }
                else if (byte.Parse(this.Configuration["Secret"]!) == 2)
                {
                    secretService = new KeyVaultService(this.Configuration);
                }

                ISupervisorLogs supervisor = new SupervisorLogs(new DalSession(secretService!, new DataContextFactory(), this.Configuration, "ConnectionStringAirZapto", "ServerTypeAirZapto"), 
                                                                new DataContextFactory(), 
                                                                new RepositoryFactory());
                supervisor.AddLogs(new Logs()
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
            switch (logEvent.Level)
            {
                case LogEventLevel.Debug:
                    return "Debug";
                case LogEventLevel.Error:
                    return "Error";
                case LogEventLevel.Fatal:
                    return "Fatal";
                case LogEventLevel.Verbose:
                    return "Verbose";
                case LogEventLevel.Warning:
                    return "Warning";
                default:
                    return "Information";
            }
        }

        static LogEventLevel SeverityToLevel(string? severity) 
        {
            switch (severity) 
            {
                case "Debug":
                    return LogEventLevel.Debug;
                case "Error":
                    return LogEventLevel.Error;
                case "Fatal":
                    return LogEventLevel.Fatal;
                case "Verbose":
                    return LogEventLevel.Verbose;
                case "Warning":
                    return LogEventLevel.Warning;
                default:
                    return LogEventLevel.Information;
            }
        }
        #endregion
    }
}
