using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace AirZapto.WebServices.Middleware
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
                //the header is necessary except for SignalR 
                if (context.Request.Headers["X-Forwarded-Path"].Equals(AirZaptoConstants.Application_Prefix)
                || (context.Request.Path.HasValue && context.Request.Path.Value.Contains(WebConstants.SignalR_Prefix)))
                {
                    await this.Next.Invoke(context);
                }
                else
                {
                    HttpResponse response = context.Response;
                    int statusCode = (int)HttpStatusCode.Forbidden;
                    string message = "Error";
                    string description = "Unauthorized Request";

                    JsonSerializerOptions options = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true,
                    };

                    response.ContentType = "application/json";
                    response.StatusCode = statusCode;
                    response.Headers.Append("exception", "messageException");

                    await response.WriteAsync(JsonSerializer.Serialize<CustomErrorResponse>(new CustomErrorResponse
                    {
                        Message = message,
                        Description = description
                    }));
                }
            }
            catch (Exception ex)
            {
                Log.Error("CustomExceptionMiddleware : " + ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        } 

        private async Task HandleExceptionAsync(HttpContext context, Exception? exception)
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
                message = customException.Message;
                description = customException.Description;
                statusCode = customException.Code;
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
