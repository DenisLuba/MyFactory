namespace MyFactory.MauiClient.Models.Warehouses;

public record WarehousesGetResponse(
    Guid Id,
    string Name,
    WarehouseType Type,
    string Location
);

