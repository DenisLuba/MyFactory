using MediatR;

namespace MyFactory.Application.Features.Warehouses.RemoveProductFromWarehouse;

public sealed record RemoveProductFromWarehouseCommand(
    Guid WarehouseId,
    Guid ProductId
) : IRequest;