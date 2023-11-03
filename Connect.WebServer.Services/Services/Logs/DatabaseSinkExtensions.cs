using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;

namespace Connect.WebServer.Services
{
    public static class DatabaseSinkExtensions
    {
        public static LoggerConfiguration DatabaseSink(this LoggerSinkConfiguration loggerConfiguration, IConfiguration configuration)
        {
            return loggerConfiguration.Sink(new DatabaseSinkService(configuration));
        }
    }
}
