using FluentValidation;
using MediatR;
using ValidationException = MyFactory.Application.Common.Exceptions.ValidationException;

namespace MyFactory.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var errors = failures
                .SelectMany(result => result.Errors)
                .Where(f => f is not null)
                .ToArray();

            if (errors.Length > 0)
            {
                var message = string.Join("; ", errors.Select(e => e.ErrorMessage));
                throw new ValidationException(message);
            }
        }

        return await next();
    }
}

