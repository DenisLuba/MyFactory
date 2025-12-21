using System;
using System.Collections.Generic;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.OldDTOs.FinishedGoods;

public sealed record ReturnDto(
    Guid Id,
    string ReturnNumber,
    Guid CustomerId,
    CustomerDto Customer,
    DateOnly ReturnDate,
    string Reason,
    string Status,
    IReadOnlyCollection<ReturnItemDto> Items)
{
    public static ReturnDto FromEntity(
        CustomerReturn customerReturn,
        CustomerDto customer,
        IReadOnlyCollection<ReturnItemDto> items)
    {
        return new ReturnDto(
            customerReturn.Id,
            customerReturn.ReturnNumber.ToString(),
            customerReturn.CustomerId,
            customer,
            customerReturn.ReturnDate,
            customerReturn.Reason,
            customerReturn.Status.ToString(),
            items);
    }
}
