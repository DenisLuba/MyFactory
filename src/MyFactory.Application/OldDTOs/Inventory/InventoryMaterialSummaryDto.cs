using System;
using System.Collections.Generic;

namespace MyFactory.Application.OldDTOs.Inventory;

public sealed record InventoryMaterialSummaryDto(
    Guid MaterialId,
    string MaterialName,
    string MaterialUnit,
    decimal TotalQuantity,
    decimal ReservedQuantity,
    decimal AvailableQuantity,
    decimal AveragePrice,
    decimal TotalValue,
    IReadOnlyCollection<InventoryWarehouseBreakdownDto> Warehouses);
