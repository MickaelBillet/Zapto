namespace Zapto.Web.Dashboard.Services
{
	public class ApplicationLoggerProvider : ILoggerProvider
    {
        private HttpClient HttpClient { get; }

        public ApplicationLoggerProvider(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(this.HttpClient);
        }

        public void Dispose()
        {

        }
    }
}
