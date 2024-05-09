using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace CheckDrive.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleAsync(context, ex);
            }
        }

        private async Task HandleAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal server error. Something went wrong, please try again later.";

            if (exception is NotFound)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
            }

            await context.Response.WriteAsync(message);
            _logger.LogError($"{message}. {exception.Message}");
        }
    }
}
