using MyFactory.MauiClient.Models.Materials;

namespace MyFactory.MauiClient.Models.Inventory;

public record InventoryItemResponse(
    Guid MaterialId,
    string MaterialName,
    Guid WarehouseId,
    double Quantity,
    Units Unit,
    decimal AvgPrice,
    double ReservedQty);
