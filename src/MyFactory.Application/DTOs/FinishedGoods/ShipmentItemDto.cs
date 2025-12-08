using System;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.DTOs.FinishedGoods;

public sealed record ShipmentItemDto(
    Guid Id,
    Guid SpecificationId,
    string SpecificationName,
    decimal Quantity,
    decimal UnitPrice,
    decimal LineTotal)
{
    public static ShipmentItemDto FromEntity(ShipmentItem item, string specificationName)
    {
        return new ShipmentItemDto(
            item.Id,
            item.SpecificationId,
            specificationName,
            item.Quantity,
            item.UnitPrice,
            item.LineTotal);
    }
}
