using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;

namespace TaskList.WebApi.Filters;

public sealed class FluentValidationActionFilter : IAsyncActionFilter
{
    private readonly ILogger<FluentValidationActionFilter> _logger;

    public FluentValidationActionFilter(ILogger<FluentValidationActionFilter> logger)
    {
        _logger = logger;
    }

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