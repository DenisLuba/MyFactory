using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Infrastructure.Common;
using MyFactory.Infrastructure.Persistence;
using MyFactory.Infrastructure.Services;

namespace MyFactory.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Settings>(configuration.GetSection("Settings"));
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddSingleton<IFileStorage, LocalFileStorage>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<InitialDataSeeder>();

        return services;
    }
}
