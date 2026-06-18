# MyFactory – AI Agent Guide

## Architecture Snapshot
- Solution layers: `MyFactory.Domain` (entities/invariants) ← `MyFactory.Application` (CQRS handlers, DTOs) ← `MyFactory.Infrastructure` (EF Core, persistence/services) ← `MyFactory.WebApi` (HTTP API).
- Keep dependency direction strict: `WebApi -> Infrastructure -> Application -> Domain`.
- `MyFactory.MauiClient` is a separate UI client and should consume the API contracts/endpoints, not backend internals.

## Domain Layer
- Entity hierarchy is `BaseEntity -> AuditableEntity -> ActivatableEntity`.
- Domain uses `IsActive` (activate/deactivate) rather than `IsDeleted` soft-delete semantics.
- Guard invariants with domain guards/exceptions; keep business rules inside entities/value objects.

## Application Layer
- Features are grouped under `Application/Features/<Module>/<UseCase>` with command/query + handler + validator nearby.
- Inject `IApplicationDbContext` in handlers; use `.AsNoTracking()` in read-only queries.
- Reuse DTO projection helpers under `Application/DTOs/**` (for example `FromEntity`) instead of duplicating mapping logic.
- Pipeline behaviors live in `Application/Common/Behaviors` and are wired by `AddApplication()` in `Application/Common/Services/AddApplicationServices.cs`.

## Infrastructure Layer
- `AddInfrastructureServices` registers Npgsql `ApplicationDbContext`, `IApplicationDbContext`, `IFileStorage` (`LocalFileStorage`), auth/user services, and `InitialDataSeeder`.
- There is no generic repository/specification layer in current codebase; prefer the existing `DbContext`-first approach.
- File persistence root comes from `Settings.FileStorageRoot`.

## Web API Conventions
- Keep contract-first flow: define request/response records under `WebApi/Contracts/**`, then keep `WebApi/ApiContract.md` in sync.
- Pair controller endpoints with examples under `WebApi/SwaggerExamples/**`.
- Startup executes seeding via `InitialDataSeeder`, and seeder applies EF migrations (`Database.MigrateAsync()`) before demo data.
- Current `Program.cs` registers MediatR directly; if you need validation/logging/transaction pipeline behaviors, switch registration to `AddApplication()`.

## MAUI Client
- `MauiProgram.cs` configures a singleton `HttpClient` (default `http://localhost:5237`). Update base address when API host/port changes.
- Keep MAUI models/services aligned with API contracts to avoid serialization drift.
- Known naming inconsistency: legacy `Expence*` naming exists in parts of client/API surface; preserve compatibility unless a full rename is planned.

## Build, Run, and Database
- Build solution: `dotnet build MyFactory.sln`.
- Run API: from `src/MyFactory.WebApi`, execute `dotnet run`.
- MAUI build examples:
	- Windows: `dotnet build src/MyFactory.MauiClient/MyFactory.MauiClient.csproj -f net10.0-windows10.0.19041.0`
	- Android: `dotnet build src/MyFactory.MauiClient/MyFactory.MauiClient.csproj -f net10.0-android`
- EF migrations from `src/MyFactory.Infrastructure`:
	- `dotnet ef migrations add <Name> --startup-project ../MyFactory.WebApi`
	- `dotnet ef database update --startup-project ../MyFactory.WebApi`

## Documentation Links
- API endpoints and payload contracts: `src/MyFactory.WebApi/ApiContract.md`
- Deployment/update flow: `src/MyFactory.WebApi/update-webapi.md`
- Domain business notes (RU):
	- `src/MyFactory.Domain/ТЗ.txt`
	- `src/MyFactory.Domain/UI экраны.md`
	- `src/MyFactory.Domain/Логика Application и UI для некоторых таблиц.md`

## Common Pitfalls
- Do not break layer dependencies (especially `Domain` must stay independent).
- In read handlers, missing `.AsNoTracking()` often causes unnecessary tracking overhead.
- If validators/behaviors seem ignored, verify MediatR registration path (`AddApplication()` vs direct `AddMediatR`).
- Keep environment-specific storage path in mind (`Settings.FileStorageRoot` may differ between local and Docker env).
- Build/publish backend with the multi-stage `Dockerfile` flow; avoid ad-hoc local publish artifacts for deployment.
