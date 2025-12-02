using MyFactory.MauiClient.Models.Materials;

namespace MyFactory.MauiClient.Models.Inventory;

public record InventoryItemResponse(
    Guid MaterialId,
    string MaterialName,
    Guid WarehouseId,
    string WarehouseName,
    double Quantity,
    Units Unit,
    decimal AvgPrice,
    decimal TotalAmount,
    double ReservedQty);
