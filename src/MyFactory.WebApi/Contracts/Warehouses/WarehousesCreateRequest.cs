namespace MyFactory.WebApi.Contracts.Warehouses;

public record WarehousesCreateRequest(
    string Name,
    WarehouseType Type,
    string Location
);

