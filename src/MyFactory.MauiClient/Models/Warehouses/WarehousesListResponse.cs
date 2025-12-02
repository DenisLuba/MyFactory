using System;

namespace MyFactory.MauiClient.Models.Warehouses;

public record WarehousesListResponse(
    Guid Id,
    string Code,
    string Name,
    WarehouseType Type,
    WarehouseStatus Status
);
