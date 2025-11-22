namespace MyFactory.WebApi.Contracts.Inventory;

public record AdjustInventoryRequest(
    Guid MaterialId,
    Guid WarehouseId,
    double NewQuantity,
    string Reason,
    DateTime AdjustmentDate
);
