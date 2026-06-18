using MediatR;

namespace MyFactory.Application.Features.Warehouses.ActivateWarehouse;

public record ActivateWarehouseCommand(Guid WarehouseId) : IRequest;
