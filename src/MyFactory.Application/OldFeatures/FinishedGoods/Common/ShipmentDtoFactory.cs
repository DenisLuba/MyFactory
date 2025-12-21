using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Common;

internal static class ShipmentDtoFactory
{
    public static Task<ShipmentDto> CreateAsync(
        IApplicationDbContext context,
        Shipment shipment,
        CancellationToken cancellationToken)
    {
        return CreateAsync(context, new[] { shipment }, cancellationToken)
            .ContinueWith(task => task.Result.First(), cancellationToken);
    }

    public static async Task<IReadOnlyCollection<ShipmentDto>> CreateAsync(
        IApplicationDbContext context,
        IReadOnlyCollection<Shipment> shipments,
        CancellationToken cancellationToken)
    {
        if (shipments.Count == 0)
        {
            return Array.Empty<ShipmentDto>();
        }

        var shipmentList = shipments.ToList();
        var customerIds = shipmentList
            .Select(shipment => shipment.CustomerId)
            .Distinct()
            .ToList();

        var specificationIds = shipmentList
            .SelectMany(shipment => shipment.Items.Select(item => item.SpecificationId))
            .Distinct()
            .ToList();

        var customers = await context.Customers
            .AsNoTracking()
            .Where(customer => customerIds.Contains(customer.Id))
            .ToDictionaryAsync(customer => customer.Id, cancellationToken);

        var specifications = await context.Specifications
            .AsNoTracking()
            .Where(specification => specificationIds.Contains(specification.Id))
            .ToDictionaryAsync(specification => specification.Id, cancellationToken);

        return shipmentList
            .Select(shipment => Map(shipment, customers, specifications))
            .ToList();
    }

    private static ShipmentDto Map(
        Shipment shipment,
        IReadOnlyDictionary<Guid, Customer> customers,
        IReadOnlyDictionary<Guid, Specification> specifications)
    {
        var customerDto = customers.TryGetValue(shipment.CustomerId, out var customer)
            ? CustomerDto.FromEntity(customer)
            : new CustomerDto(shipment.CustomerId, string.Empty, string.Empty);

        var itemDtos = shipment.Items
            .Select(item => ShipmentItemDto.FromEntity(
                item,
                specifications.TryGetValue(item.SpecificationId, out var specification) ? specification.Name : string.Empty))
            .ToList();

        return ShipmentDto.FromEntity(shipment, customerDto, itemDtos);
    }
}
