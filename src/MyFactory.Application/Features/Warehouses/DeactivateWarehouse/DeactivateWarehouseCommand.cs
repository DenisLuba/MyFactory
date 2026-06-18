using MediatR;

namespace MyFactory.Application.Features.Warehouses.DeactivateWarehouse;

public sealed record DeactivateWarehouseCommand(
    Guid WarehouseId
) : IRequest;