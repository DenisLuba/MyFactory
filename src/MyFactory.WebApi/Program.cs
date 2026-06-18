using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFactory.Application;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Infrastructure.Common;
using MyFactory.Infrastructure.Extensions;
using MyFactory.Infrastructure.Persistence;
using MyFactory.WebApi.Contracts.Auth;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginRequest>();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

        options.TokenValidationParameters = new()
        {
            ValidIssuer = jwt?.Issuer,
            ValidAudience = jwt?.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt?.Key ?? "")),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

await SeedDatabaseAsync(app);

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        (int status, string title, string? detail) = exception switch
        {
            NotFoundException ex => (StatusCodes.Status404NotFound, "Not Found", ex.Message),
            ValidationException ex => (StatusCodes.Status400BadRequest, "Validation Failed", ex.Message),
            DomainApplicationException ex => (StatusCodes.Status409Conflict, "Domain Error", ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", exception?.Message ?? "Internal Server Error")
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        var problem = Results.Problem(
            title: title,
            statusCode: status,
            detail: detail);

        await problem.ExecuteAsync(context);
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

static async Task SeedDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Seeder");
    try
    {
        var seeder = scope.ServiceProvider.GetRequiredService<InitialDataSeeder>();
        await seeder.SeedAsync();
    }
    catch (Exception exception)
    {
        logger.LogError(exception, "An error occurred while seeding the database");
        throw;
    }
}
