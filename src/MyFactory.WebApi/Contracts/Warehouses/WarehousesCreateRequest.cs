namespace MyFactory.WebApi.Contracts.Warehouses;

public record WarehousesCreateRequest(
    string Code,
    string Name,
    WarehouseType Type,
    string Location,
    WarehouseStatus Status
);

