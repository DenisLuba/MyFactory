using MediatR;

namespace MyFactory.Application.Features.Warehouses.UpdateWarehouseMaterialQty;

public sealed record UpdateWarehouseMaterialQtyCommand(
    Guid WarehouseId,
    Guid MaterialId,
    decimal Qty
) : IRequest;