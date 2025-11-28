namespace MyFactory.MauiClient.Models.Inventory;

public record AdjustInventoryRequest(
    Guid MaterialId,
    Guid WarehouseId,
    double NewQuantity,
    string Reason,
    DateTime AdjustmentDate);
