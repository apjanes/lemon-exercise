using Microsoft.AspNetCore.Mvc;
using TaskList.WebApi.Types;

namespace TaskList.WebApi.Extensions;

public static class HttpContextExtensions
{
    public static ProblemDetails ToProblemDetails(this HttpContext context, int statusCode, string title, string? detail = null, IDictionary<string, string[]>? errors = null)
    {
        var details = new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Type = ProblemTypes.GetProblemType(statusCode),
            Detail = detail,
            Instance = context.Request.Path,
        };

        details.Extensions["traceId"] = context.TraceIdentifier;
        if (errors != null) details.Extensions["errors"] = errors;

        return details;
    }
}