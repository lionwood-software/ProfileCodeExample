using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Profile.Core.SharedKernel.Exceptions;

namespace Profile.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger logger;
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            logger.LogError(exception, "Request {method}: {path} failed", context.Request.Method, context.Request.Path.Value);

            ProfileProblemDetails problemDetails;

            switch (exception)
            {
                case NotFoundObjectException:
                    problemDetails = new ProfileProblemDetails
                    {
                        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                        Title = "Not found object",
                        Status = StatusCodes.Status404NotFound,
                        Instance = context.Request.Path,
                        TraceId = Activity.Current?.TraceId.ToString()
                    };
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case UpdateAvatarException:
                    problemDetails = new ProfileProblemDetails
                    {
                        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                        Title = "Update Avatar Exception",
                        Status = StatusCodes.Status400BadRequest,
                        Instance = context.Request.Path,
                        TraceId = Activity.Current?.TraceId.ToString()
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                default:
                    problemDetails = new ProfileProblemDetails
                    {
                        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Instance = context.Request.Path,
                        TraceId = Activity.Current?.TraceId.ToString()
                    };
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(problemDetails));
        }

        private class ProfileProblemDetails : ProblemDetails
        {
            [JsonPropertyName("errorCode")]
            public string ErrorCode { get; set; }

            [JsonPropertyName("traceId")]
            public string TraceId { get; set; }
        }
    }
}
