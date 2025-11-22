namespace MyFactory.WebApi.Contracts.Inventory;

public record InventoryItemResponse(
    Guid MaterialId,
    string MaterialName,
    Guid WarehouseId,
    double Quantity,
    string Unit,
    decimal AvgPrice,
    double ReservedQty
);
