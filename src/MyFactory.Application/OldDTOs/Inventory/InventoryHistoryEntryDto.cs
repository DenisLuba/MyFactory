using System;

namespace MyFactory.Application.OldDTOs.Inventory;

public sealed record InventoryHistoryEntryDto(
    Guid ReceiptId,
    string ReceiptNumber,
    DateOnly ReceiptDate,
    Guid SupplierId,
    string SupplierName,
    Guid MaterialId,
    string MaterialName,
    Guid? WarehouseId,
    string? WarehouseName,
    decimal Quantity,
    decimal UnitPrice,
    decimal LineTotal,
    string Status);
