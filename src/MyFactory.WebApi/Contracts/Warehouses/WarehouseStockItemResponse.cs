namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record WarehouseStockItemResponse(
    Guid ItemId,
    string Name,
    decimal QtyPerPackage,
    decimal? PackageCount,
    decimal TotalQty,
    string? UnitCode);
