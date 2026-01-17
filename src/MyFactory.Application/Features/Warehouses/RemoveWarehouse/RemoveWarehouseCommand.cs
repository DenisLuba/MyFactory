using MediatR;

namespace MyFactory.Application.Features.Warehouses.RemoveWarehouse;

public record RemoveWarehouseCommand(Guid WarehouseId) : IRequest;
