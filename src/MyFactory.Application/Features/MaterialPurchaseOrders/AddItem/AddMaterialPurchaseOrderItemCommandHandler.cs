using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.AddItem;

public sealed class AddMaterialPurchaseOrderItemCommandHandler
    : IRequestHandler<AddMaterialPurchaseOrderItemCommand>
{
    private readonly IApplicationDbContext _db;

    public AddMaterialPurchaseOrderItemCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
    AddMaterialPurchaseOrderItemCommand request,
    CancellationToken cancellationToken)
{
    var order = await _db.MaterialPurchaseOrders
        .Include(o => o.MaterialPurchaseItems)
        .FirstOrDefaultAsync(o => o.Id == request.PurchaseOrderId, cancellationToken);

    if (order is null)
        throw new NotFoundException("Purchase order not found");

    order.EnsureEditable();

    var material = await _db.Materials
        .Include(m => m.Unit)
        .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken);

    if (material is null)
        throw new NotFoundException("Material not found");

    if (material.Unit is null)
        throw new DomainApplicationException("Material has no unit");

    var item = new MaterialPurchaseOrderItemEntity(
        purchaseOrderId: order.Id,
        materialId: material.Id,
        qty: request.Qty,
        unitPrice: request.UnitPrice,
        materialName: material.Name,
        unitCode: material.Unit.Code
    );

    _db.MaterialPurchaseOrderItems.Add(item);

    await _db.SaveChangesAsync(cancellationToken);
}

}
