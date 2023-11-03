using Connect.WebServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Connect.Server.Configuration
{
    public static class LoggerExtension
    {
        public static ILogger ConfigureLogger(this IApplicationBuilder app, IConfiguration configuration)
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
