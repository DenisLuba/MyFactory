using System;
using MyFactory.Domain.Entities.FinishedGoods;

namespace MyFactory.Application.DTOs.FinishedGoods;

public sealed record FinishedGoodsMovementDto(
    Guid Id,
    Guid SpecificationId,
    string SpecificationName,
    Guid FromWarehouseId,
    string FromWarehouseName,
    Guid ToWarehouseId,
    string ToWarehouseName,
    Guid? FinishedGoodsInventoryId,
    decimal Quantity,
    DateTime MovedAt)
{
    public static FinishedGoodsMovementDto FromEntity(
        FinishedGoodsMovement movement,
        string specificationName,
        string fromWarehouseName,
        string toWarehouseName)
    {
        return new FinishedGoodsMovementDto(
            movement.Id,
            movement.SpecificationId,
            specificationName,
            movement.FromWarehouseId,
            fromWarehouseName,
            movement.ToWarehouseId,
            toWarehouseName,
            movement.FinishedGoodsInventoryId,
            movement.Quantity,
            movement.MovedAt);
    }
}
