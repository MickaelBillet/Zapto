using Serilog;
using ILogger = Serilog.ILogger;
using WeatherZapto.WebServer.Services;

namespace WeatherZapto.WebServer.Configuration
{
    public static class LoggerExtension
    {
        public static ILogger UseSerilog(this IApplicationBuilder app, IConfiguration configuration)
        {
            return new LoggerConfiguration()
                                    .ReadFrom.Configuration(configuration)
                                    .Enrich.FromLogContext()
                                    .WriteTo.DatabaseSink(configuration)
                                    .Filter.ByExcluding("RequestPath like '%/health%'")
                                    .CreateLogger();
        }
    }
}
