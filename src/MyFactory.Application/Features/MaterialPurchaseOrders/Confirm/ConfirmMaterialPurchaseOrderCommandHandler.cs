using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Confirm;

public sealed class ConfirmMaterialPurchaseOrderCommandHandler
    : IRequestHandler<ConfirmMaterialPurchaseOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public ConfirmMaterialPurchaseOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        ConfirmMaterialPurchaseOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _db.MaterialPurchaseOrders
            .Include(o => o.MaterialPurchaseItems)
            .FirstOrDefaultAsync(o => o.Id == request.PurchaseOrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException("Purchase order not found");

        order.Confirm();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
