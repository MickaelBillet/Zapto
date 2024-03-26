using Framework.Infrastructure.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AirZapto.WebServer
{
    public partial class Program
	{
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
                                                                 .SetBasePath(Directory.GetCurrentDirectory())
                                                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                                 .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
                                                                 .AddEnvironmentVariables()
                                                                 .Build();

        public static IWebHostBuilder BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
                                                                    .UseUrls("http://*:6001")
                                                                    .UseStartup<Startup>()
                                                                    .UseKestrel()
                                                                    .UseContentRoot(Directory.GetCurrentDirectory());

		public static async Task Main(string[] args)
        {
            try
            {
                IWebHost host = BuildWebHost(args).Build();
                var startupTasks = host.Services.GetServices<IStartupTask>();
                foreach (var task in startupTasks)
                {
                    await task.Execute();
                }
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }        
    }
}
