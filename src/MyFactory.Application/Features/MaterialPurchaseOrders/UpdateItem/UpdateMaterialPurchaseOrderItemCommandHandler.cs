using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.UpdateItem;

public sealed class UpdateMaterialPurchaseOrderItemCommandHandler
    : IRequestHandler<UpdateMaterialPurchaseOrderItemCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateMaterialPurchaseOrderItemCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateMaterialPurchaseOrderItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _db.MaterialPurchaseOrderItems
            .Include(i => i.MaterialPurchaseOrder)
            .FirstOrDefaultAsync(i => i.Id == request.ItemId, cancellationToken);

        if (item is null)
            throw new NotFoundException("Purchase order item not found");

        if (item.MaterialPurchaseOrder is null)
            throw new DomainException("Purchase order for item not loaded");

        item.UpdateQty(request.Qty, item.MaterialPurchaseOrder);
        item.UpdateUnitPrice(request.UnitPrice, item.MaterialPurchaseOrder);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
