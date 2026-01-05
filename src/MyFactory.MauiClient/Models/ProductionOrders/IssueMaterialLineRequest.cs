namespace MyFactory.MauiClient.Models.ProductionOrders;

public record IssueMaterialLineRequest(
    Guid MaterialId,
    Guid WarehouseId,
    decimal Qty);
