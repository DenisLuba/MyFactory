namespace MyFactory.MauiClient.Models.Materials;

public record WarehouseQtyResponse(
    Guid WarehouseId,
    string WarehouseName,
    decimal Qty,
    string UnitCode);
