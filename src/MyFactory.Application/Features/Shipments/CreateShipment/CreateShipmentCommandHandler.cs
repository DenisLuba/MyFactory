using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.Shipments.CreateShipment;

public sealed class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateShipmentCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.SalesOrders
            .Include(o => o.SalesOrderItems)
            .FirstOrDefaultAsync(o => o.Id == request.SalesOrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException("Sales order not found");

        if (order.CustomerId != request.CustomerId)
            throw new ValidationException("Customer does not match the sales order");

        var customerExists = await _db.Customers
            .AsNoTracking()
            .AnyAsync(c => c.Id == request.CustomerId, cancellationToken);

        if (!customerExists)
            throw new NotFoundException("Customer not found");

        // Validate items belong to order and stock availability
        var orderItemsById = order.SalesOrderItems.ToDictionary(i => i.Id, i => i);

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();

        var stocks = await _db.FinishedGoodsStocks
            .Where(s => productIds.Contains(s.ProductId))
            .ToListAsync(cancellationToken);

        foreach (var item in request.Items)
        {
            if (!orderItemsById.TryGetValue(item.SalesOrderItemId, out var orderItem))
                throw new ValidationException($"Sales order item '{item.SalesOrderItemId}' not found in order.");

            if (orderItem.ProductId != item.ProductId)
                throw new ValidationException("Product does not match sales order item");

            var stock = stocks.FirstOrDefault(s => s.ProductId == item.ProductId && s.WarehouseId == item.WarehouseId);
            if (stock is null)
                throw new ValidationException("No stock for product on selected warehouse");

            if (item.Qty > stock.Qty)
                throw new ValidationException("Not enough stock to ship the requested quantity");
        }

        var shipment = new ShipmentEntity(request.SalesOrderId, request.CustomerId, request.ShipmentDate, request.CreatedBy);

        await _db.Shipments.AddAsync(shipment, cancellationToken);

        foreach (var item in request.Items)
        {
            var shipmentItem = new ShipmentItemEntity(
                shipment.Id,
                item.SalesOrderItemId,
                item.WarehouseId,
                item.ProductId,
                item.Qty,
                item.UnitPrice);

            await _db.ShipmentItems.AddAsync(shipmentItem, cancellationToken);

            var stock = stocks.First(s => s.ProductId == item.ProductId && s.WarehouseId == item.WarehouseId);
            stock.RemoveQty(item.Qty);
        }

        // apply initial status if provided
        if (request.Status is not null)
        {
            switch (request.Status)
            {
                case 
                    DTOs.Shipments.ShipmentStatus.Draft:
                    // already draft
                    break;
                case DTOs.Shipments.ShipmentStatus.Confirmed:
                    shipment.Confirm();
                    break;
                case DTOs.Shipments.ShipmentStatus.Shipped:
                    shipment.Confirm();
                    shipment.Ship();
                    break;
                case DTOs.Shipments.ShipmentStatus.Cancelled:
                    shipment.Cancel();
                    break;
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        return shipment.Id;
    }
}
