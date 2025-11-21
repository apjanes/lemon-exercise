namespace TaskList.WebApi.Types;

public static class ProblemTypes
{
    private const string Base = "https://docs.tasklist.app/problems";

    private static readonly Dictionary<int, string> Slugs = new Dictionary<int, string>
    {
        { StatusCodes.Status400BadRequest, BadRequest },
        { StatusCodes.Status401Unauthorized, Unauthorized },
        { StatusCodes.Status403Forbidden, Forbidden },
        { StatusCodes.Status404NotFound, NotFound },
        { StatusCodes.Status409Conflict, Conflict },
        { StatusCodes.Status422UnprocessableEntity, UnprocessableEntity },
        { StatusCodes.Status429TooManyRequests, TooManyRequests },
        { StatusCodes.Status500InternalServerError, UnexpectedError },
        { StatusCodes.Status503ServiceUnavailable, ServiceUnavailable },
    };

    public const string BadRequest = "bad-request";
    public const string Unauthorized = "unauthorized";
    public const string Forbidden = "forbidden";
    public const string NotFound = "not-found";
    public const string Conflict = "conflict";
    public const string UnprocessableEntity = "unprocessable-entity";
    public const string TooManyRequests = "too-many-requests";
    public const string UnexpectedError = "unexpected-error";
    public const string ServiceUnavailable = "service-unavailable";

    public static string GetProblemType(int statusCode)
    {
        if (Slugs.TryGetValue(statusCode, out var slug))
        {
            return $"{Base}/{slug}";
        }

        return $"{Base}/unknown";
    }
}