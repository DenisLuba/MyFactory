using MyFactory.WebApi.Contracts.Materials;

namespace MyFactory.WebApi.Contracts.Inventory;

public record InventoryItemResponse(
    Guid MaterialId,
    string MaterialName,
    Guid WarehouseId,
    double Quantity,
    Units Unit,
    decimal AvgPrice,
    double ReservedQty
);
