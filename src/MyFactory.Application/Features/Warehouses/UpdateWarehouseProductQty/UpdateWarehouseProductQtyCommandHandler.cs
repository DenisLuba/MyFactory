using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Warehouses.UpdateWarehouseProductQty;

public sealed class UpdateWarehouseProductQtyCommandHandler
    : IRequestHandler<UpdateWarehouseProductQtyCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public UpdateWarehouseProductQtyCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        UpdateWarehouseProductQtyCommand request,
        CancellationToken cancellationToken)
    {
        var stock = await _db.FinishedGoodsStocks
            .FirstOrDefaultAsync(
                x => x.WarehouseId == request.WarehouseId &&
                     x.ProductId == request.ProductId,
                cancellationToken)
            ?? throw new NotFoundException("Product not found in warehouse");

        var previousTotal = stock.Qty;
        var newTotal = CalculateTotal(request.QtyPerPackage, request.PackageCount);
        var delta = Math.Abs(newTotal - previousTotal);

        stock.AdjustQty(request.QtyPerPackage, request.PackageCount);

        if (delta > 0m)
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
                    qty: delta)); 
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private static decimal CalculateTotal(decimal qtyPerPackage, decimal? packageCount)
        => packageCount is null ? qtyPerPackage : qtyPerPackage * packageCount.Value;
}
