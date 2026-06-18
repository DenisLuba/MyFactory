using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;
using System.Linq;

namespace MyFactory.Application.Features.Warehouses.AddProductToWarehouse;

public sealed class AddProductToWarehouseCommandHandler
    : IRequestHandler<AddProductToWarehouseCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AddProductToWarehouseCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(AddProductToWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _db.Warehouses
            .FirstOrDefaultAsync(x => x.Id == request.WarehouseId, cancellationToken)
            ?? throw new NotFoundException("Warehouse not found");

        if (warehouse.Type != WarehouseType.FinishedGoods)
            throw new ValidationException("Can only add products to a finished goods warehouse");

        var productExists = await _db.Products
            .AnyAsync(x => x.Id == request.ProductId, cancellationToken);

        if (!productExists)
            throw new NotFoundException("Product not found");

        var stocks = await _db.FinishedGoodsStocks
            .Where(x => x.WarehouseId == request.WarehouseId && x.ProductId == request.ProductId)
            .ToListAsync(cancellationToken);

        if (stocks.Count == 0)
        {
            _db.FinishedGoodsStocks.Add(
                new FinishedGoodsStockEntity(
                    request.WarehouseId,
                    request.ProductId,
                    request.QtyPerPackage,
                    request.PackageCount));
        }
        else
        {
            var primaryStock = stocks[0];

            if (stocks.Count > 1)
            {
                var duplicateTotal = stocks.Skip(1).Sum(s => s.Qty);
                _db.FinishedGoodsStocks.RemoveRange(stocks.Skip(1));

                if (duplicateTotal > 0m)
                    primaryStock.AddQty(duplicateTotal);
            }

            primaryStock.AddQty(request.QtyPerPackage, request.PackageCount);
        }

        var qty = request.PackageCount is null ? request.QtyPerPackage : request.QtyPerPackage * request.PackageCount.Value;

        await CreateAdjustmentMovement(request.WarehouseId, request.ProductId, qty, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateAdjustmentMovement(
    Guid warehouseId,
    Guid productId,
    decimal qty,
    CancellationToken cancellationToken)
    {
        var movement = new FinishedGoodsMovementEntity(
            FinishedGoodsMovementType.Adjustment,
            warehouseId,
            null,
            DateTime.UtcNow,
            _currentUser.UserId);

        _db.FinishedGoodsMovements.Add(movement);

        _db.FinishedGoodsMovementItems.Add(
            new FinishedGoodsMovementItemEntity(
                movement.Id,
                productId,
                qty));

        await Task.CompletedTask;
    }
}
