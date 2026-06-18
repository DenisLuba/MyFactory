using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Shipments.DeleteShipment;

public sealed class DeleteShipmentCommandHandler : IRequestHandler<DeleteShipmentCommand>
{
    private readonly IApplicationDbContext _db;

    public DeleteShipmentCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(DeleteShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _db.Shipments
            .Include(s => s.ShipmentItems)
            .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

        if (shipment is null)
            throw new NotFoundException("Shipment not found");

        foreach (var item in shipment.ShipmentItems)
        {
            var stock = await _db.FinishedGoodsStocks
                .FirstOrDefaultAsync(s => s.ProductId == item.ProductId && s.WarehouseId == item.WarehouseId, cancellationToken);

            if (stock is null)
            {
                stock = new FinishedGoodsStockEntity(item.WarehouseId, item.ProductId, 0);
                await _db.FinishedGoodsStocks.AddAsync(stock, cancellationToken);
            }

            stock.AddQty(item.Qty);
        }

        _db.ShipmentItems.RemoveRange(shipment.ShipmentItems);
        _db.Shipments.Remove(shipment);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
