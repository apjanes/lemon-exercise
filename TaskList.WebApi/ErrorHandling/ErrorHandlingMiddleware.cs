using Microsoft.AspNetCore.Mvc;
using TaskList.WebApi.Extensions;
using TaskList.WebApi.Types;

namespace TaskList.WebApi.ErrorHandling;

/// <summary>
/// Middleware that provides centralized error handling for unhandled exceptions.
/// </summary>
/// <remarks>
/// This middleware ensures that all unhandled exceptions are caught and converted into
/// a standardized <see cref="ProblemDetails"/> response (RFC7807 compliant),
/// including a <c>traceId</c> for easier correlation in logs.
/// </remarks>
public sealed class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The application logger used to record exception details.</param>
    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Invokes the next middleware in the pipeline and handles any unhandled exceptions.
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/>.</param>
    /// <param name="next">The next middleware component in the request pipeline.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <remarks>
    /// When an exception occurs, this method:
    /// <list type="bullet">
    /// <item><description>Logs the exception details at <c>Error</c> level.</description></item>
    /// <item><description>Returns a standardized <see cref="ProblemDetails"/> response with HTTP 500 status.</description></item>
    /// <item><description>Adds the current request’s <c>traceId</c> to the response for correlation.</description></item>
    /// </list>
    /// </remarks>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
#pragma warning disable CA1031 // Disable warning about catching generic Exception as this is a global handler.
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.UnhandledException(context.Request.Method, context.Request.Path, exception);

            var details = context.ToProblemDetails(
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.",
                "If the problem persists, contact support.");

            context.Response.ContentType = ContentTypes.ApplicationProblemJson;
            context.Response.StatusCode = details.Status!.Value;

            await context.Response.WriteAsJsonAsync(details);
        }
#pragma warning restore CA1031
    }
}