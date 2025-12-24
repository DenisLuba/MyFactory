using MediatR;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed record CreateWarehouseCommand(
    string Name,
    WarehouseType Type
) : IRequest<Guid>;