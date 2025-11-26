namespace MyFactory.WebApi.Contracts.Warehouses;

public record WarehousesUpdateResponse(
    Guid Id,
    WarehouseStatus Status
);

