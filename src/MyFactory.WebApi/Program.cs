using MyFactory.Application;
using MyFactory.WebApi.Contracts.Auth;
using MyFactory.WebApi.Services.Employees;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using MyFactory.Infrastructure.Persistence;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using MyFactory.Application.Interfaces.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginRequest>();

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
//builder.Services.AddAutoMapper(typeof(AssemblyMarker));

builder.Services.AddSingleton<IEmployeeRepository, InMemoryEmployeeRepository>();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});
//builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
//builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
//builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
//builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
