using System;

namespace MyFactory.WebApi.Contracts.Warehouses;

public record WarehousesListResponse(
    Guid Id,
    string Code,
    string Name,
    WarehouseType Type,
    WarehouseStatus Status
);
