using Conduit.Application.Abstractions.Results;
using FluentValidation;
using MediatR;

namespace Conduit.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct
    )
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
        {
            var validationErrors = failures
                .Select(f => new ValidationError(f.PropertyName, f.ErrorCode, f.ErrorMessage))
                .ToList();

            var error = Error.Validation(validationErrors);

            return (TResponse)(object)Result.Failure(error);
        }

        return await next();
    }
}
