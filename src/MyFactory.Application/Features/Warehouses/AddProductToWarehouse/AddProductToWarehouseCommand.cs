using MediatR;

namespace MyFactory.Application.Features.Warehouses.AddProductToWarehouse;

public sealed record AddProductToWarehouseCommand(
    Guid WarehouseId,
    Guid ProductId,
    int Qty
) : IRequest;
