using System;

namespace MyFactory.Application.OldDTOs.Inventory;

public sealed record InventoryWarehouseMaterialDto(
    Guid MaterialId,
    string MaterialName,
    string MaterialUnit,
    Guid WarehouseId,
    string WarehouseName,
    string WarehouseType,
    string WarehouseLocation,
    decimal Quantity,
    decimal ReservedQuantity,
    decimal AvailableQuantity,
    decimal AveragePrice,
    decimal TotalValue);
