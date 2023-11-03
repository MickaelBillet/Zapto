using AirZapto.WebServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace AirZapto.WebServices.Configuration
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
