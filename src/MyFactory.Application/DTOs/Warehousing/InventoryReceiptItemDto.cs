using System;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.DTOs.Warehousing;

public sealed record InventoryReceiptItemDto(
    Guid Id,
    Guid MaterialId,
    string MaterialName,
    Guid? InventoryItemId,
    decimal Quantity,
    decimal UnitPrice,
    decimal LineTotal)
{
    public static InventoryReceiptItemDto FromEntity(InventoryReceiptItem item, Material material)
        => new(item.Id, item.MaterialId, material.Name, item.InventoryItemId, item.Quantity, item.UnitPrice, item.LineTotal);
}
