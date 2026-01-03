using MediatR;
using Microsoft.Extensions.Logging;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUserService _currentUser;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        ICurrentUserService currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var user = _currentUser.IsAuthenticated ? _currentUser.UserId.ToString() : "anonymous";

        _logger.LogInformation("Handling {RequestName} by {User}", requestName, user);

        var response = await next();

        _logger.LogInformation("Handled {RequestName} by {User}", requestName, user);

        return response;
    }
}

