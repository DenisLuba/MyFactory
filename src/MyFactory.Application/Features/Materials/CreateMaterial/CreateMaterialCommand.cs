using MediatR;

namespace MyFactory.Application.Features.Materials.CreateMaterial;

public sealed record CreateMaterialCommand(string Name, Guid MaterialTypeId, Guid UnitId, string? Color, string? Desctiption = null) : IRequest<Guid>;
