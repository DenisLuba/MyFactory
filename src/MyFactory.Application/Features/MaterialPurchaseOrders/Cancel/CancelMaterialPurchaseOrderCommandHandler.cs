using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Cancel;

public sealed class CancelMaterialPurchaseOrderCommandHandler
    : IRequestHandler<CancelMaterialPurchaseOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public CancelMaterialPurchaseOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(CancelMaterialPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.MaterialPurchaseOrders
            .FirstOrDefaultAsync(o => o.Id == request.PurchaseOrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException("Purchase order not found");

        order.Cancel();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
