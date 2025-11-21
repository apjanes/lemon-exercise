namespace TaskList.WebApi.ErrorHandling;

internal static partial class ErrorHandlingLogging
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Error,
        Message = "Unhandled exception processing {Method} {Path}.")]
    public static partial void UnhandledException(this ILogger logger, string method, string path, Exception exception);

    [LoggerMessage(EventId = 1001, Level = LogLevel.Information,
        Message = "Validation failed for {Method} {Path}.")]
    public static partial void ValidationFailed(this ILogger logger, string method, string path);
}