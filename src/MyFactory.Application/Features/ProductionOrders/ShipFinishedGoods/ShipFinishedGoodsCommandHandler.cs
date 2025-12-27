using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.ShipFinishedGoods;

public sealed class ShipFinishedGoodsCommandHandler
    : IRequestHandler<ShipFinishedGoodsCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ShipFinishedGoodsCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        ShipFinishedGoodsCommand request,
        CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");

        if (po.Status != ProductionOrderStatus.Packaging &&
            po.Status != ProductionOrderStatus.Finished)
            throw new DomainException("Finished goods cannot be shipped at this stage.");

        var soi = await _db.SalesOrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == po.SalesOrderItemId, cancellationToken)
            ?? throw new NotFoundException("Sales order item not found");

        var productId = soi.ProductId;

        var stock = await _db.FinishedGoodsStocks
            .FirstOrDefaultAsync(x =>
                x.WarehouseId == request.FromWarehouseId &&
                x.ProductId == productId,
                cancellationToken)
            ?? throw new DomainException("Finished goods stock not found.");

        if (stock.Qty < request.Qty)
            throw new DomainException("Not enough finished goods in stock.");

        var movement = new FinishedGoodsMovementEntity(
            request.FromWarehouseId,
            request.ToWarehouseId,
            DateTime.UtcNow,
            _currentUser.UserId);

        _db.FinishedGoodsMovements.Add(movement);

        var movementItem = new FinishedGoodsMovementItemEntity(
            movement.Id,
            productId,
            request.Qty);

        _db.FinishedGoodsMovementItems.Add(movementItem);

        stock.RemoveQty(request.Qty);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
