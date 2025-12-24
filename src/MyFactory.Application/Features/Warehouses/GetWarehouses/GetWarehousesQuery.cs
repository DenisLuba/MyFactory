using MediatR;
using MyFactory.Application.DTOs.Warehouses;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed record GetWarehousesQuery(
    bool IncludeInactive = false
) : IRequest<IReadOnlyList<WarehouseListItemDto>>;