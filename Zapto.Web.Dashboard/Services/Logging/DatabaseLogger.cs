using Connect.Model;
using Framework.Core.Base;
using Framework.Core.Domain;
using System.Net.Http.Json;

namespace Zapto.Web.Dashboard.Services
{
    public class DatabaseLogger : ILogger
    {
        private HttpClient HttpClient { get; }

        public DatabaseLogger(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Logs log = new()
            {
                Level = logLevel.ToString(),
                Date = Clock.Now,
                Exception = exception?.Message,
                RenderedMessage = string.Format("{0}: {1} - {2}", logLevel.ToString(), eventId.Id, formatter(state, exception))
            };
            this.HttpClient.PostAsJsonAsync<Logs>(ConnectConstants.RestUrlLogs, log);
        }
    }
}
