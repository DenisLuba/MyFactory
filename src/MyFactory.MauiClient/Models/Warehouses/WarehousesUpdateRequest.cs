namespace MyFactory.MauiClient.Models.Warehouses;

public record WarehousesUpdateRequest(
    string Name,
    WarehouseType Type,
    string Location,
    WarehouseStatus Status
);

