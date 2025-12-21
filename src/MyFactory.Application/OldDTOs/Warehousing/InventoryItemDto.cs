using System;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.OldDTOs.Warehousing;

public sealed record InventoryItemDto(
    Guid Id,
    Guid WarehouseId,
    Guid MaterialId,
    string MaterialName,
    decimal Quantity,
    decimal ReservedQuantity,
    decimal AvailableQuantity,
    decimal AveragePrice)
{
    public static InventoryItemDto FromEntity(InventoryItem item, Material material)
        => new(
            item.Id,
            item.WarehouseId,
            item.MaterialId,
            material.Name,
            item.Quantity,
            item.ReservedQuantity,
            item.AvailableQuantity,
            item.AveragePrice);
}
