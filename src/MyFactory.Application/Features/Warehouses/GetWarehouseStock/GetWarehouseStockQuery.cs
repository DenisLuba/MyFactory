using MediatR;
using MyFactory.Application.DTOs.Warehouses;

namespace MyFactory.Application.Features.Warehouses.GetWarehouseStock;

public sealed record GetWarehouseStockQuery(
    Guid WarehouseId
) : IRequest<IReadOnlyList<WarehouseStockItemDto>>;