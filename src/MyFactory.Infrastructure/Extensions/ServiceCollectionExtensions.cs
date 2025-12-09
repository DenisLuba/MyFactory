using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Infrastructure.Common;
using MyFactory.Infrastructure.Persistence;
using MyFactory.Infrastructure.Persistence.Seeds;
using MyFactory.Infrastructure.Persistence.UnitOfWork;
using MyFactory.Infrastructure.Repositories;
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
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<InitialDataSeeder>();
        services.AddSingleton<IFileStorage, LocalFileStorage>();

        return services;
    }
}
