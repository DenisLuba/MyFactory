using MediatR;

namespace MyFactory.Application.Features.Warehouses.UpdateWarehouseProductQty;

public sealed record UpdateWarehouseProductQtyCommand(
    Guid WarehouseId,
    Guid ProductId,
    decimal QtyPerPackage,
    decimal? PackageCount = null
) : IRequest;