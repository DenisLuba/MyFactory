# MyFactory - AI Coding Agent Instructions

## Architecture Overview

This is a **Clean Architecture** .NET 10 solution for factory management with:
- **WebAPI** (ASP.NET Core) - REST API backend
- **MAUI Client** (cross-platform) - Mobile/desktop UI
- **Domain** - Entities, value objects, domain exceptions
- **Application** - MediatR commands/queries, behaviors, DTOs, AutoMapper profiles
- **Infrastructure** - EF Core (PostgreSQL), repositories, services

### Layer Dependencies
```
WebApi → Infrastructure → Application → Domain
MauiClient → (HTTP calls to WebApi)
```

Domain has **zero dependencies**. Application references only Domain. Infrastructure implements Application interfaces.

## Project Structure Conventions

### Domain Layer (`MyFactory.Domain`)
- `Common/BaseEntity.cs` - Base class with `Guid Id`
- `Common/ValueObject.cs` - DDD value object base
- `Common/DomainException.cs` - Domain-specific exceptions
- `Entities/` - Domain entities (currently empty, planned for Production, Materials, etc.)
- `ValueObjects/` - Domain value objects

### Application Layer (`MyFactory.Application`)
- **Assembly Marker Pattern**: `AssemblyMarker.cs` used for MediatR/AutoMapper assembly scanning
- `Features/` organized by **business domain** (not technical layers):
  - `Production/`, `Materials/`, `Inventory/`, `FinishedGoods/`
  - `Finance/`, `Payroll/`, `Advances/`
  - `Specifications/` (bill of materials, operations)
- `Common/Behaviors/` - MediatR pipeline behaviors:
  - `ValidationBehavior` (FluentValidation)
  - `LoggingBehavior` (cross-cutting logging)
  - `TransactionBehavior` (database transactions)
- `Common/Interfaces/` - Abstractions: `IApplicationDbContext`, `IDateTimeProvider`
- `DTOs/` - Data transfer objects

### Infrastructure Layer (`MyFactory.Infrastructure`)
- `Persistence/ApplicationDbContext.cs` - EF Core DbContext (PostgreSQL)
- `Persistence/Configurations/` - Entity configurations
- `Persistence/Migrations/` - EF Core migrations
- `Repositories/` - Repository implementations
- `Services/` - External service implementations
- `Common/JwtOptions.cs`, `Common/Settings.cs` - Configuration

### WebAPI Layer (`MyFactory.WebApi`)
- **Contract-First API Design**: All request/response types in `Contracts/` folder
- **Swagger Examples**: Every contract has example provider in `SwaggerExamples/`
  - Implements `IExamplesProvider<T>` from Swashbuckle.AspNetCore.Filters
  - Example: `LoginRequestExample` provides sample data for `LoginRequest`
- `Controllers/` organized by business domain matching Application features
- Controllers use **record types** for contracts: `record LoginRequest(string Username, string Password)`
- Middleware folder: `MIddlewares/` (note the typo - preserve it)

### MAUI Client (`MyFactory.MauiClient`)
- Targets: Android, iOS, MacCatalyst, Windows
- Uses **CommunityToolkit.Maui** and **CommunityToolkit.Mvvm**
- **Refit** for HTTP API client
- `ViewModels/` organized by feature (Reference, Production, Warehouse, Finance, Specifications)
- ViewModels are **plain classes** (not currently using ObservableObject pattern)
- `Pages/`, `Models/`, `Services/`, `Converters/`, `UIModels/`

## Technology Stack

- **.NET 10.0** (latest)
- **MediatR 13.1.0** - CQRS pattern (Application/Infrastructure)
- **AutoMapper 12.0.1** - Object mapping (Application)
- **FluentValidation 12.1.0** - Request validation
- **EF Core 9.0.4** with **Npgsql** (PostgreSQL provider)
- **Serilog** - Structured logging
- **JWT Bearer Authentication** (configured but not implemented)
- **Swashbuckle 10.0.1** with Filters - OpenAPI/Swagger
- **xUnit** - Testing framework

## Development Workflows

### Building the Solution
```powershell
dotnet build MyFactory.sln
```

### Running the API
```powershell
cd src\MyFactory.WebApi
dotnet run
# API available at https://localhost:5001 (check console output)
```

### Running Tests
```powershell
dotnet test  # Run all test projects
dotnet test tests/MyFactory.Application.Tests
```

### Database Migrations
```powershell
cd src\MyFactory.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../MyFactory.WebApi
dotnet ef database update --startup-project ../MyFactory.WebApi
```

### Running MAUI Client
```powershell
cd src\MyFactory.MauiClient
dotnet build -f net10.0-windows10.0.19041.0  # Windows
dotnet build -f net10.0-android              # Android
```

## Code Patterns & Conventions

### MediatR Commands/Queries
While the structure is set up for CQRS, **Features folders are currently empty**. When implementing:
- Place in `Application/Features/{Domain}/Commands/` or `.../Queries/`
- Use `record` for immutable commands: `record CreateProductCommand(string Name, decimal Price)`
- Handlers return `Task<Result<T>>` or domain-specific types

### API Controllers
- Use `[ApiController]` and `[Route("api/{resource}")]`
- **Always** add `[Produces("application/json")]`
- Use `[SwaggerRequestExample]` and `[SwaggerResponseExample]` for every endpoint
- Return typed responses: `[ProducesResponseType(typeof(TResponse), StatusCode)]`
- Use `record` types for contracts (immutable, concise)

Example pattern from `AuthController`:
```csharp
[HttpPost("login")]
[Consumes("application/json")]
[ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
[SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
public IActionResult Login([FromBody] LoginRequest req) => Ok(new LoginResponse(...));
```

### Swagger Example Providers
Every request/response in `Contracts/` should have a corresponding example:
```csharp
public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples() => new("admin", "P@ssw0rd");
}
```

### Entity Configuration
- Use `IEntityTypeConfiguration<T>` in Infrastructure
- Place in `Persistence/Configurations/`
- Configure relationships, indexes, constraints explicitly

### Dependency Registration
- Application services registered in WebApi `Program.cs`
- Uses **AssemblyMarker pattern**: `typeof(AssemblyMarker).Assembly` for scanning
- Infrastructure services should use extension methods (not yet implemented)

## Important Notes

- **Project is in early stages**: Most domain entities, handlers, and repositories are **not implemented yet**
- **No connection string configured**: PostgreSQL database setup needed
- **Authentication is stubbed**: JWT infrastructure in place but endpoints return mock data
- **Test projects are empty**: Only boilerplate `UnitTest1.cs` exists
- **ValueObject and DomainException classes are empty shells** - implement pattern as needed
- Folder typo: `MIddlewares` (capital 'I') - preserve for consistency

## Business Domains

The factory system manages:
- **Materials** - Raw materials, suppliers, purchases, receipts, stock
- **Production** - Orders, specifications, operations, transfers, shifts
- **Finished Goods** - Receipts, shipments, returns, inventory
- **Finance** - Overheads, advances, advance reports
- **Payroll** - Shift-based worker payments
- **Specifications** - Bill of Materials (BOM), operations, costs
- **Warehouses** - Stock management, inventory tracking

## Testing Strategy

Tests organized by layer:
- `MyFactory.Domain.Tests` - Domain logic unit tests
- `MyFactory.Application.Tests` - Application handler tests
- `MyFactory.Integration.Tests` - API integration tests

Use xUnit with coverlet for code coverage.
