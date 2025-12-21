using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Application.Features.FinishedGoods.Common;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Commands.CreateShipment;

public sealed class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, ShipmentDto>
{
    private readonly IApplicationDbContext _context;

    public CreateShipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShipmentDto> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        if (request.Items.Count == 0)
        {
            throw new InvalidOperationException("Shipment must contain at least one item.");
        }

        var customerExists = await _context.Customers
            .AsNoTracking()
            .AnyAsync(customer => customer.Id == request.CustomerId, cancellationToken);

        if (!customerExists)
        {
            throw new InvalidOperationException("Customer not found.");
        }

        var shipmentNumberExists = await _context.Shipments
            .AsNoTracking()
            .AnyAsync(shipment => shipment.ShipmentNumber == request.ShipmentNumber, cancellationToken);

        if (shipmentNumberExists)
        {
            throw new InvalidOperationException("Shipment number already exists.");
        }

        var specificationIds = request.Items
            .Select(item => item.SpecificationId)
            .Distinct()
            .ToList();

        var specificationCount = await _context.Specifications
            .AsNoTracking()
            .CountAsync(specification => specificationIds.Contains(specification.Id), cancellationToken);

        if (specificationCount != specificationIds.Count)
        {
            throw new InvalidOperationException("One or more specifications were not found.");
        }

        var shipment = new Shipment(request.ShipmentNumber, request.CustomerId, request.ShipmentDate);

        foreach (var item in request.Items)
        {
            shipment.AddItem(item.SpecificationId, item.Quantity, item.UnitPrice);
        }

        await _context.Shipments.AddAsync(shipment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return await ShipmentDtoFactory.CreateAsync(_context, shipment, cancellationToken);
    }
}
