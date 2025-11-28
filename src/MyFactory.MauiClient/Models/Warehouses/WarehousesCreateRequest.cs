namespace MyFactory.MauiClient.Models.Warehouses;

public record WarehousesCreateRequest(
    string Name,
    WarehouseType Type,
    string Location
);

