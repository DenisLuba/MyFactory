using System;
using System.Collections.Generic;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.DTOs.FinishedGoods;

public sealed record ShipmentDto(
    Guid Id,
    string ShipmentNumber,
    Guid CustomerId,
    CustomerDto Customer,
    DateOnly ShipmentDate,
    string Status,
    decimal TotalAmount,
    IReadOnlyCollection<ShipmentItemDto> Items)
{
    public static ShipmentDto FromEntity(
        Shipment shipment,
        CustomerDto customer,
        IReadOnlyCollection<ShipmentItemDto> items)
    {
        return new ShipmentDto(
            shipment.Id,
            shipment.ShipmentNumber.ToString(),
            shipment.CustomerId,
            customer,
            shipment.ShipmentDate,
            shipment.Status.ToString(),
            shipment.TotalAmount,
            items);
    }
}
