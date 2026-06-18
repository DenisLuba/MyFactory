using Microsoft.Extensions.DependencyInjection;
using MyFactory.Application.Common.Behaviors;
using FluentValidation;

namespace MyFactory.Application.Common.Services;

public static class AddApplicationServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });
        services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);
        return services;
    }
}
