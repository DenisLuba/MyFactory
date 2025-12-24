using MediatR;

namespace MyFactory.Application.Features.Warehouses.AddMaterialToWarehouse;

public sealed record AddMaterialToWarehouseCommand(
    Guid WarehouseId,
    Guid MaterialId,
    decimal Qty
) : IRequest;
