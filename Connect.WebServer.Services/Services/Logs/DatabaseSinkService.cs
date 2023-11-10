using Connect.Data;
using Connect.Data.DataContext;
using Connect.Data.Repositories;
using Connect.Data.Supervisors;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;
using Framework.Core.Domain;
using Connect.Data.Session;

namespace Connect.WebServer.Services
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
                this.Level = SeverityToLevel(configuration["Serilog.MinimumLevel"]);
                this.Configuration = configuration;
            }
        }
        #endregion

        #region Methods
        public void Emit(LogEvent logEvent)
        {
            if ((this.Configuration != null) && (logEvent.Level >= this.Level))
            {
                ISupervisorLog supervisor = new SupervisorLog(new DalSession(new DataContextFactory(), this.Configuration), new RepositoryFactory());

                //Why I don't have the warning CS4014 
                supervisor.AddLog(new Logs()
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
