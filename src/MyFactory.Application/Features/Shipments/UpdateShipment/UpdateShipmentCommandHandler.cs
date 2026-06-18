using System;
using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Shipments;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.Shipments.UpdateShipment;

public sealed class UpdateShipmentCommandHandler : IRequestHandler<UpdateShipmentCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public UpdateShipmentCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(UpdateShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _db.Shipments
            .Include(s => s.ShipmentItems)
            .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

        if (shipment is null)
            throw new NotFoundException("Shipment not found");

        // Update shipment date
        if (request.ShipmentDate is DateTime date && date != default)
        {
            shipment.GetType().GetProperty(nameof(shipment.ShipmentDate))?.SetValue(shipment, date);
        }

        // Handle status change
        if (request.Status is not null)
        {
            var target = request.Status.ShipmentToDomainStatus();

            switch (target)
            {
                case Domain.Entities.Orders.ShipmentStatus.Draft:
                    // No action; creation already sets Draft, no direct domain method to revert
                    break;
                case Domain.Entities.Orders.ShipmentStatus.Confirmed:
                    if (shipment.Status == Domain.Entities.Orders.ShipmentStatus.Draft)
                        shipment.Confirm();
                    break;
                case Domain.Entities.Orders.ShipmentStatus.Shipped:
                    if (shipment.Status == Domain.Entities.Orders.ShipmentStatus.Draft)
                        shipment.Confirm();
                    if (shipment.Status != Domain.Entities.Orders.ShipmentStatus.Shipped)
                        shipment.Ship();
                    break;
                case Domain.Entities.Orders.ShipmentStatus.Cancelled:
                    if (shipment.Status != Domain.Entities.Orders.ShipmentStatus.Cancelled)
                        shipment.Cancel();
                    break;
            }
        }

        // Update items if provided
        if (request.Items is not null)
        {
            var salesOrderItems = await _db.SalesOrderItems
                .AsNoTracking()
                .Where(soi => soi.SalesOrderId == shipment.SalesOrderId)
                .Select(soi => new { soi.Id, soi.ProductId })
                .ToDictionaryAsync(x => x.Id, x => x.ProductId, cancellationToken);

            // restore stock from existing shipment items
            foreach (var existing in shipment.ShipmentItems)
            {
                var stock = await _db.FinishedGoodsStocks
                    .FirstOrDefaultAsync(s => s.ProductId == existing.ProductId && s.WarehouseId == existing.WarehouseId, cancellationToken);

                if (stock is null)
                {
                    stock = new FinishedGoodsStockEntity(existing.WarehouseId, existing.ProductId, 0);
                    await _db.FinishedGoodsStocks.AddAsync(stock, cancellationToken);
                }

                stock.AddQty(existing.Qty);
            }

            _db.ShipmentItems.RemoveRange(shipment.ShipmentItems);

            // validate and apply new items
            var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
            var stocks = await _db.FinishedGoodsStocks
                .Where(s => productIds.Contains(s.ProductId))
                .ToListAsync(cancellationToken);

            foreach (var item in request.Items)
            {
                if (!salesOrderItems.TryGetValue(item.SalesOrderItemId, out var productId) || productId != item.ProductId)
                    throw new ValidationException("Shipment item must reference an existing sales order item with matching product");

                var stock = stocks.FirstOrDefault(s => s.ProductId == item.ProductId && s.WarehouseId == item.WarehouseId);
                if (stock is null)
                    throw new ValidationException("No stock available for the specified warehouse/product");

                if (item.Qty > stock.Qty)
                    throw new ValidationException("Not enough stock to ship the requested quantity");

                stock.RemoveQty(item.Qty);

                var shipmentItem = new ShipmentItemEntity(
                    shipment.Id,
                    item.SalesOrderItemId,
                    item.WarehouseId,
                    item.ProductId,
                    item.Qty,
                    item.UnitPrice);

                await _db.ShipmentItems.AddAsync(shipmentItem, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        return shipment.Id;
    }
}
