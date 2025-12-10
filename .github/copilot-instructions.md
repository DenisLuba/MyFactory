# MyFactory – AI Agent Guide
## Architecture Snapshot
- Solution layers: `MyFactory.Domain` (entities/invariants) ← `Application` (MediatR, DTOs) ← `Infrastructure` (EF Core, repositories, file storage) ← `WebApi` (HTTP surface) plus the MAUI client (`MyFactory.MauiClient`) that only calls the API.
- Follow the dependency rule `WebApi → Infrastructure → Application → Domain`; adding a reference in the opposite direction will break the intended Clean Architecture split.
- `MyFactory.WebApi/Program.cs` wires controllers, Swagger, Swashbuckle examples, infrastructure services, and a temporary `InMemoryEmployeeRepository`; MediatR/AutoMapper registrations are commented out until real handlers are ready.

## Domain Layer
- Every aggregate inherits `Common/BaseEntity` (Guid `Id`, timestamps, `IsDeleted`); guard invariants with helpers under `Common/Guard` or dedicated value objects.
- Soft delete/audit dates are applied centrally from `ApplicationDbContext.ApplyAuditInformation`; avoid duplicating this logic inside entities.

## Application Layer
- Commands/queries live beside their handlers and validators (see `Features/Materials/Commands/CreateMaterial`). Use `record` types and inject `IApplicationDbContext` or repositories; default read handlers should call `.AsNoTracking()`.
- DTOs under `Application/DTOs/**` offer `FromEntity` helpers—reuse them for projections to prevent spread-out mapping code.
- Cross-cutting behaviors belong in `Common/Behaviors`; once MediatR is enabled ensure logging/validation pipeline behaviors are registered.

## Infrastructure Layer
- `Extensions/ServiceCollectionExtensions.AddInfrastructureServices` binds `Settings`/`Jwt` config, registers `ApplicationDbContext` (Npgsql), repository abstractions (`IRepository<T>`, `IMaterialRepository`), `IUnitOfWork`, `InitialDataSeeder`, and the `IFileStorage` implementation (`LocalFileStorage`).
- Repository implementations follow a specification pattern (`Infrastructure/Repositories/**`). Use `Specification<T>` helpers and `PaginatedResult.Create(...)` for paging; call `.IncludeDeleted()` when soft-deleted rows must be queried.
- File uploads/downloads must flow through `IFileStorage`; persisted files land beneath `Settings.FileStorageRoot` (relative path unless fully qualified).

## Web API Conventions
- Contract-first: create request/response records inside `WebApi/Contracts/**`, document them in `ApiContract.md`, and pair controllers with Swagger examples from `SwaggerExamples/**`.
- Controllers live under `WebApi/Controllers/**`. Match `AuthController` or `EmployeesController` for routing, filters, and `[ProducesResponseType]` annotations; keep the intentional `MIddlewares` casing untouched.
- Startup seeds data via `SeedDatabaseAsync` which resolves `InitialDataSeeder`. Toggle demo data and storage roots through `Settings` in `appsettings*.json`.

## MAUI Client
- `MauiProgram.cs` adds every service/viewmodel/page through chained extension methods and configures a singleton `HttpClient` pointing at `http://localhost:5237`. Update `BaseAddress` if the API host/port changes.
- Client models under `MauiClient/Models/**` mirror Web API contracts exactly—edit both sides together to avoid serialization issues. The `MyFactory.MauiClient.Core` project holds shared abstractions (models, services) reused by the MAUI shell.

## Build, Run, and Database
- Build everything with `dotnet build MyFactory.sln`. Run the API from `src\MyFactory.WebApi` via `dotnet run`; this also applies migrations and executes the seeder.
- MAUI builds: `dotnet build src\MyFactory.MauiClient/MyFactory.MauiClient.csproj -f net10.0-windows10.0.19041.0` (Windows) or `-f net10.0-android`.
- EF migrations: from `src\MyFactory.Infrastructure` execute `dotnet ef migrations add <Name> --startup-project ../MyFactory.WebApi`, then `dotnet ef database update --startup-project ../MyFactory.WebApi`.

## Testing & Quality Gates
- Tests live under `tests/**` per layer (`MyFactory.Application.Tests`, `MyFactory.Domain.Tests`, etc.) but mostly contain scaffolding `UnitTest1`. Add xUnit specs inside the matching project when implementing real features.
- Many controllers/service methods still return stubs; confirm the intended behavior in the Application layer or `ApiContract.md` before assuming existing responses.
