namespace MyFactory.WebApi.Contracts.Warehouses;

public record WarehousesGetResponse(
    Guid Id,
    string Name,
    WarehouseType Type,
    string Location
);

