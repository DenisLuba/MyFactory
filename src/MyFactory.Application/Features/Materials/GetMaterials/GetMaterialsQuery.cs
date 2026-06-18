using MediatR;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterials;

public record GetMaterialsQuery(
    string? SearchName = null,
    string? SearchType = null,
    string? SortBy = null,
    bool SortDesc = false,
    int Skip = 0,
    int Take = 30,
    bool IsActive = true,
    Guid? WarehouseId = null
) : IRequest<ListDto<MaterialListItemDto>>;
