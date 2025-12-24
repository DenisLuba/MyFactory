using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Warehouses.TransferProducts;

public sealed class TransferProductsCommandHandler
    : IRequestHandler<TransferProductsCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public TransferProductsCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        TransferProductsCommand request,
        CancellationToken cancellationToken)
    {
        var movement = new FinishedGoodsMovementEntity(
            request.FromWarehouseId,
            request.ToWarehouseId,
            DateTime.UtcNow,
            _currentUser.UserId
        );

        _db.FinishedGoodsMovements.Add(movement);

        foreach (var item in request.Items)
        {
            var fromStock = await _db.FinishedGoodsStocks
                .FirstOrDefaultAsync(
                    x => x.WarehouseId == request.FromWarehouseId &&
                         x.ProductId == item.ProductId,
                    cancellationToken)
                ?? throw new DomainException("Product not found in source warehouse.");

            fromStock.RemoveQty(item.Qty);

            var toStock = await _db.FinishedGoodsStocks
                .FirstOrDefaultAsync(
                    x => x.WarehouseId == request.ToWarehouseId &&
                         x.ProductId == item.ProductId,
                    cancellationToken);

            if (toStock == null)
            {
                toStock = new FinishedGoodsStockEntity(
                    request.ToWarehouseId,
                    item.ProductId,
                    item.Qty);

                _db.FinishedGoodsStocks.Add(toStock);
            }
            else
            {
                toStock.AddQty(item.Qty);
            }

            var movementItem = new FinishedGoodsMovementItemEntity(
                movement.Id,
                item.ProductId,
                item.Qty);

            _db.FinishedGoodsMovementItems.Add(movementItem);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}