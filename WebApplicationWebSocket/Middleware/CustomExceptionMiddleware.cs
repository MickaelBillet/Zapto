using Framework.Core.Base;
using Serilog;
using System.Net;
using System.Text.Json;

namespace WebApplicationWebSocket.Middleware
{
    public class CustomExceptionMiddleware
    {
        #region Properties
        private RequestDelegate Next { get; }
        #endregion

        #region Constructor
        public CustomExceptionMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }
        #endregion

        #region Methods
        public async Task Invoke(HttpContext context)
        {
            try
            {
                
			}
            catch (Exception ex)
            {
                Log.Error("CustomExceptionMiddleware : " + ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpResponse response = context.Response;
            BaseCustomException? customException = exception as BaseCustomException;
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Unexpected error";
            string description = "Unexpected error";

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };

            if (null != customException)
            {
                message     = customException.Message;
                description = customException.Description;
                statusCode  = customException.Code;
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            response.Headers.Append("exception", "messageException");

            await response.WriteAsync(JsonSerializer.Serialize<CustomErrorResponse>(new CustomErrorResponse
            {
                Message = message,
                Description = description
            }));
        }
        #endregion
    }
}
