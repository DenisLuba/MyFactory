using System;
using MyFactory.Domain.Entities.FinishedGoods;

namespace MyFactory.Application.OldDTOs.FinishedGoods;

public sealed record FinishedGoodsInventoryDto(
    Guid Id,
    Guid SpecificationId,
    string SpecificationName,
    Guid WarehouseId,
    string WarehouseName,
    decimal Quantity,
    decimal UnitCost,
    DateTime UpdatedAt)
{
    public static FinishedGoodsInventoryDto FromEntity(
        FinishedGoodsInventory inventory,
        string specificationName,
        string warehouseName)
    {
        return new FinishedGoodsInventoryDto(
            inventory.Id,
            inventory.SpecificationId,
            specificationName,
            inventory.WarehouseId,
            warehouseName,
            inventory.Quantity,
            inventory.UnitCost,
            inventory.UpdatedAt);
    }
}
