using MediatR;

namespace MyFactory.Application.Features.Warehouses.AddMaterialToWarehouse;

public sealed record AddMaterialToWarehouseCommand(
    Guid WarehouseId,
    Guid MaterialId,
    decimal QtyPerPackage,
    decimal? PackageCount = null
) : IRequest;
