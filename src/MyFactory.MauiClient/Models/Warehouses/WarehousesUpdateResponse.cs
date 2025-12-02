using System;

namespace MyFactory.MauiClient.Models.Warehouses;

public record WarehousesUpdateResponse(
    Guid Id,
    WarehouseStatus Status
);

