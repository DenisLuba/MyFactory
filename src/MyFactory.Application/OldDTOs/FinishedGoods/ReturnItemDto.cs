using System;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.OldDTOs.FinishedGoods;

public sealed record ReturnItemDto(
    Guid Id,
    Guid SpecificationId,
    string SpecificationName,
    decimal Quantity,
    string Disposition)
{
    public static ReturnItemDto FromEntity(CustomerReturnItem item, string specificationName)
    {
        return new ReturnItemDto(
            item.Id,
            item.SpecificationId,
            specificationName,
            item.Quantity,
            item.Disposition);
    }
}
