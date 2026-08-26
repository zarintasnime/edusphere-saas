using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AssignmentSubmissionManagementSystem.API.Filters;

/// <summary>
/// Runs the FluentValidation validators that are registered in the Application layer.
///
/// The validators were already registered via AddValidatorsFromAssembly, but nothing ever
/// invoked them, so every rule in Application/Validators was dead code. This filter resolves
/// IValidator&lt;T&gt; for each action argument and validates it before the action runs.
///
/// Implemented as a filter rather than the FluentValidation.AspNetCore package because that
/// package's auto-validation was removed in FluentValidation 11+ (this project is on 12.x).
/// </summary>
public sealed class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var failures = new Dictionary<string, List<string>>();

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>)
                .MakeGenericType(argument.GetType());

            if (_serviceProvider.GetService(validatorType) is not IValidator validator)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(argument);

            var result = await validator.ValidateAsync(validationContext);

            if (result.IsValid)
            {
                continue;
            }

            foreach (var error in result.Errors)
            {
                var key = ToCamelCase(error.PropertyName);

                if (!failures.TryGetValue(key, out var messages))
                {
                    messages = new List<string>();
                    failures[key] = messages;
                }

                messages.Add(error.ErrorMessage);
            }
        }

        if (failures.Count > 0)
        {
            throw new ValidationFailedException(
                failures.ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value.ToArray()));
        }

        await next();
    }

    private static string ToCamelCase(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || char.IsLower(propertyName[0]))
        {
            return propertyName;
        }

        return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
    }
}
