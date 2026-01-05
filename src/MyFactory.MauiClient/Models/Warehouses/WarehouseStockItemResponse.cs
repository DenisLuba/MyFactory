namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record WarehouseStockItemResponse(Guid ItemId, string Name, decimal Qty, string? UnitCode);
