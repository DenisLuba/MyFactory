namespace MyFactory.WebApi.Contracts.Warehouses;

public record WarehousesUpdateRequest(
    string Name,
    WarehouseType Type,
    string Location,
    WarehouseStatus Status
);

