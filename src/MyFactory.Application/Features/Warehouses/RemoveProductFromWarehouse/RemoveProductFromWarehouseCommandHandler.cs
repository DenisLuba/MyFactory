using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Warehouses.RemoveProductFromWarehouse;

public sealed class RemoveProductFromWarehouseCommandHandler
    : IRequestHandler<RemoveProductFromWarehouseCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public RemoveProductFromWarehouseCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        RemoveProductFromWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        var stock = await _db.FinishedGoodsStocks
            .FirstOrDefaultAsync(
                x => x.WarehouseId == request.WarehouseId &&
                     x.ProductId == request.ProductId,
                cancellationToken)
            ?? throw new NotFoundException("Product not found in warehouse");

        if (stock.Qty > 0)
        {
            var movement = new FinishedGoodsMovementEntity(
                movementType: FinishedGoodsMovementType.Adjustment,
                fromWarehouseId: request.WarehouseId,
                toWarehouseId: null,
                movementDate: DateTime.UtcNow,
                createdBy: _currentUser.UserId);

            _db.FinishedGoodsMovements.Add(movement);

            _db.FinishedGoodsMovementItems.Add(
                new FinishedGoodsMovementItemEntity(
                    movementId: movement.Id,
                    productId: request.ProductId,
                    qty: stock.Qty));
        }

        _db.FinishedGoodsStocks.Remove(stock);

        await _db.SaveChangesAsync(cancellationToken);
    }
}

