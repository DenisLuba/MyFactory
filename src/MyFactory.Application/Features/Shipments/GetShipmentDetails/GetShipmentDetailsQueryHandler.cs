using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Shipments;

namespace MyFactory.Application.Features.Shipments.GetShipmentDetails;

public sealed class GetShipmentDetailsQueryHandler : IRequestHandler<GetShipmentDetailsQuery, ShipmentDetailsDto?>
{
    private readonly IApplicationDbContext _db;

    public GetShipmentDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ShipmentDetailsDto?> Handle(GetShipmentDetailsQuery request, CancellationToken cancellationToken)
    {
        var shipment = await _db.Shipments
            .AsNoTracking()
            .Include(s => s.ShipmentItems)
            .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

        if (shipment is null)
            throw new NotFoundException("Shipment not found");

        var items = shipment.ShipmentItems
            .Select(i => new ShipmentDetailsItemDto(
                ShipmentItemId: i.Id,
                SalesOrderItemId: i.SalesOrderItemId,
                ProductId: i.ProductId,
                WarehouseId: i.WarehouseId,
                Qty: i.Qty,
                UnitPrice: i.UnitPrice))
            .ToList();

        return new ShipmentDetailsDto(
            Id: shipment.Id,
            SalesOrderId: shipment.SalesOrderId,
            CustomerId: shipment.CustomerId,
            ShipmentDate: shipment.ShipmentDate,
            Status: ShipmentStatusExtensions.DomainToShipmentStatus(shipment.Status),
            Items: items);
    }
}

