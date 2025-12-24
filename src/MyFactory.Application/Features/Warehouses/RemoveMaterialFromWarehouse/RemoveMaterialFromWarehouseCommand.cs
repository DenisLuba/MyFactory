using MediatR;

namespace MyFactory.Application.Features.Warehouses.RemoveMaterialFromWarehouse;

public sealed record RemoveMaterialFromWarehouseCommand(
    Guid WarehouseId,
    Guid MaterialId
) : IRequest;