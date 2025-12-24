using MediatR;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed record UpdateWarehouseCommand(
    Guid WarehouseId,
    string Name,
    WarehouseType Type
) : IRequest;