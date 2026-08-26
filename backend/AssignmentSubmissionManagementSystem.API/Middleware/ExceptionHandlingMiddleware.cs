using System.Text.Json;
using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;

namespace AssignmentSubmissionManagementSystem.API.Middleware;

/// <summary>
/// Turns every exception into one predictable JSON shape:
///     { "message": "...", "statusCode": 401, "errors": { "email": ["..."] } }
///
/// Without this, a wrong password produced an unhandled 500 (or an HTML developer
/// exception page), which the React client had no way to render sensibly.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleAsync(context, exception);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception is AppException app ? app.StatusCode : 500;

        var message = exception is AppException
            ? exception.Message
            : "An unexpected error occurred. Please try again.";

        var errors = exception is ValidationFailedException validation
            ? validation.Errors
            : null;

        if (statusCode >= 500)
        {
            _logger.LogError(
                exception,
                "Unhandled exception on {Method} {Path}",
                context.Request.Method,
                context.Request.Path);
        }
        else
        {
            _logger.LogWarning(
                "Returning {StatusCode} for {Method} {Path}: {Message}",
                statusCode,
                context.Request.Method,
                context.Request.Path,
                message);
        }

        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = new ErrorResponse
        {
            Message = message,
            StatusCode = statusCode,
            Errors = errors,
            Detail = _environment.IsDevelopment() && statusCode >= 500
                ? exception.ToString()
                : null
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(payload, JsonOptions));
    }

    private sealed class ErrorResponse
    {
        public string Message { get; init; } = string.Empty;

        public int StatusCode { get; init; }

        public IDictionary<string, string[]>? Errors { get; init; }

        public string? Detail { get; init; }
    }
}


public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}
