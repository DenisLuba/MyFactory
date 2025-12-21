using MediatR;

namespace MyFactory.Application.Features.Materials.UpdateMaterial;

public sealed record UpdateMaterialCommand : IRequest
{
    public Guid MaterialId { get; init; }

    public string Name { get; init; } = default!;
    public Guid MaterialTypeId { get; init; }
    public Guid UnitId { get; init; }
    public string? Color { get; init; }
}


