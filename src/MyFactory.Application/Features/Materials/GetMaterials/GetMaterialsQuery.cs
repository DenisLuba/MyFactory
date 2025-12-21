using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterials;

public sealed class MaterialFilter
{
    public string? MaterialName { get; init; }
    public string? MaterialType { get; init; }
    public bool? IsActive { get; init; }
    public Guid? WarehouseId { get; init; }
}

public record GetMaterialsQuery(
    MaterialFilter? Filter = null
) : IRequest<IReadOnlyList<MaterialListItemDto>>;
