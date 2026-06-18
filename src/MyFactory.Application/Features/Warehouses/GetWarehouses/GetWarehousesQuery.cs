using MediatR;
using MyFactory.Application.DTOs.Warehouses;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed record GetWarehousesQuery(
    bool IncludeInactive = false,
    WarehouseType[]? WarehouseTypes = null
) : IRequest<IReadOnlyList<WarehouseListItemDto>>;