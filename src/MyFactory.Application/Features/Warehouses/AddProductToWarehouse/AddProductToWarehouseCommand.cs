using MediatR;

namespace MyFactory.Application.Features.Warehouses.AddProductToWarehouse;

public sealed record AddProductToWarehouseCommand(
    Guid WarehouseId,
    Guid ProductId,
    decimal QtyPerPackage,
    decimal? PackageCount = null
) : IRequest;
