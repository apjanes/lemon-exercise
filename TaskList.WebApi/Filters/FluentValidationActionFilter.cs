using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;

namespace TaskList.WebApi.Filters;

/// <summary>
/// An ASP.NET Core action filter that automatically runs FluentValidation validators
/// against incoming action parameters before the controller action executes.
/// </summary>
/// <remarks>
/// This filter uses dependency injection to resolve <see cref="IValidator{T}"/> instances
/// for each action argument. If validation fails, the request is short-circuited and
/// a <see cref="BadRequestObjectResult"/> containing a <see cref="ValidationProblemDetails"/>
/// is returned to the client.
/// </remarks>
public sealed class FluentValidationActionFilter : IAsyncActionFilter
{
    private readonly ILogger<FluentValidationActionFilter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentValidationActionFilter"/> class.
    /// </summary>
    /// <param name="logger">The logger used to record validation events or errors.</param>
    public FluentValidationActionFilter(ILogger<FluentValidationActionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Executes validation logic before the target action is invoked.
    /// </summary>
    /// <param name="context">The current <see cref="ActionExecutingContext"/> containing action arguments and HTTP context.</param>
    /// <param name="next">The delegate to execute the next filter or action if validation passes.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var http = context.HttpContext;
        var services = http.RequestServices;
        var cancellationToken = http.RequestAborted;

        var errorBag = new ConcurrentDictionary<string, List<string>>();
        foreach (var (argName, argValue) in context.ActionArguments)
        {
            if (argValue is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argValue.GetType());
            var validatorObj = services.GetService(validatorType) as IValidator;
            if (validatorObj is null) continue;

            var validationContext = new ValidationContext<object>(argValue);
            ValidationResult result = await validatorObj.ValidateAsync(validationContext, cancellationToken);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    var key = string.IsNullOrWhiteSpace(failure.PropertyName)
                        ? argName
                        : failure.PropertyName;

                    errorBag.AddOrUpdate(
                        key,
                        _ => new List<string> { failure.ErrorMessage },
                        (_, list) => { list.Add(failure.ErrorMessage); return list; });
                }
            }
        }

        if (!errorBag.IsEmpty)
        {
            var problem = new ValidationProblemDetails(
                errorBag.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray()))
            {
                Title = "Validation failed",
                Status = StatusCodes.Status400BadRequest
            };

            context.Result = new BadRequestObjectResult(problem);
            return;
        }

        await next();
    }
}