using System;

namespace MyFactory.WebApi.Contracts.Warehouses;

public record WarehousesCreateResponse(
    Guid Id,
    WarehouseStatus Status
);

