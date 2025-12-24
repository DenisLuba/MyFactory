using MediatR;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed record DeactivateWarehouseCommand(
    Guid WarehouseId
) : IRequest;