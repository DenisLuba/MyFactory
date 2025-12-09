using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFactory.Infrastructure.Extensions;
using MyFactory.Infrastructure.Persistence.Seeds;
using MyFactory.WebApi.Contracts.Auth;
using MyFactory.WebApi.Services.Employees;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginRequest>();

builder.Services.AddInfrastructureServices(builder.Configuration);

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
//builder.Services.AddAutoMapper(typeof(AssemblyMarker));

builder.Services.AddSingleton<IEmployeeRepository, InMemoryEmployeeRepository>();

var app = builder.Build();

await SeedDatabaseAsync(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

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
