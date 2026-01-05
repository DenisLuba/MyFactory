namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record WarehouseStockItemResponse(Guid ItemId, string Name, decimal Qty, string? UnitCode);
