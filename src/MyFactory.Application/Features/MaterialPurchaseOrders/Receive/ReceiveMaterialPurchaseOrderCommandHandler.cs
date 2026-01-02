using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Receive;

public sealed class ReceiveMaterialPurchaseOrderCommandHandler
    : IRequestHandler<ReceiveMaterialPurchaseOrderCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ReceiveMaterialPurchaseOrderCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }
    public async Task Handle(
        ReceiveMaterialPurchaseOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _db.MaterialPurchaseOrders
            .Include(o => o.MaterialPurchaseItems)
            .FirstOrDefaultAsync(o => o.Id == request.PurchaseOrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException("Purchase order not found");

        // 1. Меняем статус заказа
        order.Receive();

        // 2. Создаём движение (приход на склад)
        var movement = new InventoryMovementEntity(
            movementType: InventoryMovementType.Receipt,
            fromWarehouseId: null,
            toWarehouseId: request.WarehouseId,
            toDepartmentId: null,
            productionOrderId: null,
            createdBy: _currentUser.UserId
        );

        _db.InventoryMovements.Add(movement);

        // 3. Для каждого item — движение + обновление склада
        foreach (var item in order.MaterialPurchaseItems)
        {
            // 3.1 Movement item
            var movementItem = new InventoryMovementItemEntity(
                movementId: movement.Id,
                materialId: item.MaterialId,
                qty: item.Qty,
                unitCost: item.UnitPrice
            );

            _db.InventoryMovementItems.Add(movementItem);

            // 3.2 Обновление склада
            var stock = await _db.WarehouseMaterials
                .FirstOrDefaultAsync(
                    wm => wm.WarehouseId == request.WarehouseId
                    && wm.MaterialId == item.MaterialId,
                    cancellationToken);

            if (stock is null)
            {
                stock = new WarehouseMaterialEntity(
                    warehouseId: request.WarehouseId,
                    materialId: item.MaterialId,
                    qty: item.Qty
                );

                _db.WarehouseMaterials.Add(stock);
            }
            else
            {
                stock.AddQty(item.Qty);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
