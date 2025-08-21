using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using InventoryReservation.Application.Common.Exceptions;

namespace InventoryReservation.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            string title = "An error occurred";

            switch (exception)
            {
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    title = "Resource not found";
                    break;
                case ValidationException:
                case ArgumentException:
                    code = HttpStatusCode.BadRequest;
                    title = "Bad request";
                    break;
                case InvalidOperationException:
                    code = HttpStatusCode.Conflict; // business rule violation
                    title = "Conflict";
                    break;
            }

            var problem = new
            {
                type = $"https://httpstatuses.io/{(int)code}",
                title,
                status = (int)code,
                detail = exception.Message
            };

            var payload = JsonSerializer.Serialize(problem);
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(payload);
        }
    }
}
