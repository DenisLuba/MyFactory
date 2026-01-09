using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.RemoveItem;

public sealed class RemoveMaterialPurchaseOrderItemCommandHandler
    : IRequestHandler<RemoveMaterialPurchaseOrderItemCommand>
{
    private readonly IApplicationDbContext _db;

    public RemoveMaterialPurchaseOrderItemCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RemoveMaterialPurchaseOrderItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _db.MaterialPurchaseOrderItems
            .Include(i => i.MaterialPurchaseOrder)
            .FirstOrDefaultAsync(i => i.Id == request.ItemId, cancellationToken);

        if (item is null)
            throw new NotFoundException("Purchase order item not found");

        if (item.MaterialPurchaseOrder is null)
            throw new DomainException("Purchase order for item not loaded");

        item.MaterialPurchaseOrder.EnsureEditable();

        _db.MaterialPurchaseOrderItems.Remove(item);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
