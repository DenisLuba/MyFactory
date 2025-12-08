using System;
using System.Collections.Generic;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.DTOs.FinishedGoods;

public sealed record ReturnDto(
    Guid Id,
    string ReturnNumber,
    Guid CustomerId,
    CustomerDto Customer,
    DateTime ReturnDate,
    string Reason,
    ReturnStatus Status,
    IReadOnlyCollection<ReturnItemDto> Items)
{
    public static ReturnDto FromEntity(
        CustomerReturn customerReturn,
        CustomerDto customer,
        IReadOnlyCollection<ReturnItemDto> items)
    {
        return new ReturnDto(
            customerReturn.Id,
            customerReturn.ReturnNumber,
            customerReturn.CustomerId,
            customer,
            customerReturn.ReturnDate,
            customerReturn.Reason,
            customerReturn.Status,
            items);
    }
}
