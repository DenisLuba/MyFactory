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

        var fromWarehouse = await _db.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == request.FromWarehouseId);

        if (fromWarehouse?.Type is not WarehouseType.FinishedGoods)
            throw new DomainApplicationException("Mistake. Moving products is not from a warehouse of products.");

        var toWarehouse = await _db.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == request.ToWarehouseId);

        if (toWarehouse?.Type is not WarehouseType.FinishedGoods)
            throw new DomainApplicationException("Mistake. Moving products is not to a warehouse of products.");

        var movement = new FinishedGoodsMovementEntity(
            FinishedGoodsMovementType.Transfer,
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
                ?? throw new DomainApplicationException("Product not found in source warehouse.");

            fromStock.RemoveQty(item.QtyPerPackage, item.PackageCount);

            var toStock = await _db.FinishedGoodsStocks
                .FirstOrDefaultAsync(
                    x => x.WarehouseId == request.ToWarehouseId &&
                         x.ProductId == item.ProductId,
                    cancellationToken);

            if (toStock is null)
            {
                toStock = new FinishedGoodsStockEntity(
                    request.ToWarehouseId,
                    item.ProductId,
                    item.QtyPerPackage,
                    item.PackageCount);

                _db.FinishedGoodsStocks.Add(toStock);
            }
            else
            {
                toStock.AddQty(item.QtyPerPackage, item.PackageCount);
            }

            var transferTotal = CalculateTotal(item.QtyPerPackage, item.PackageCount);

            var movementItem = new FinishedGoodsMovementItemEntity(
                movement.Id,
                item.ProductId,
                transferTotal);

            _db.FinishedGoodsMovementItems.Add(movementItem);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private static decimal CalculateTotal(decimal qty, decimal? packageCount)
    => packageCount is null ? qty : qty * packageCount.Value;
}