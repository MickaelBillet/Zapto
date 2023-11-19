using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using System.Threading.RateLimiting;

namespace WeatherZapto.WebServer.Middleware
{
    public class RateLimiterPolicy : IRateLimiterPolicy<string>
    {
        #region Constructor
        public RateLimiterPolicy()
        {

        }
        #endregion

        #region Methods
        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get; } =
        (context, _) =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            Log.Error($"Request rejected by {nameof(RateLimiterPolicy)}");
            return new ValueTask();
        };

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                return RateLimitPartition.GetSlidingWindowLimiter(httpContext.User.Identity.Name!,
                    partition => new SlidingWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 40,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        SegmentsPerWindow = 40,
                    });
            }

            return RateLimitPartition.GetSlidingWindowLimiter(httpContext.Request.Headers.Host.ToString(),
                partition => new SlidingWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 20,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    SegmentsPerWindow = 40,
                });
        }
        #endregion
    }
}
