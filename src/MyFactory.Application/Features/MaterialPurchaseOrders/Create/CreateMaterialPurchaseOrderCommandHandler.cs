using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Create;

public sealed class CreateMaterialPurchaseOrderCommandHandler
    : IRequestHandler<CreateMaterialPurchaseOrderCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateMaterialPurchaseOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        CreateMaterialPurchaseOrderCommand request,
        CancellationToken cancellationToken)
    {
        var supplierExists = await _db.Suppliers
            .AnyAsync(s => s.Id == request.SupplierId && s.IsActive, cancellationToken);

        if (!supplierExists)
            throw new NotFoundException("Supplier not found or inactive");

        var order = MaterialPurchaseOrderEntity.Create(
            supplierId: request.SupplierId,
            orderDate: request.OrderDate
        );

        _db.MaterialPurchaseOrders.Add(order);
        await _db.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
