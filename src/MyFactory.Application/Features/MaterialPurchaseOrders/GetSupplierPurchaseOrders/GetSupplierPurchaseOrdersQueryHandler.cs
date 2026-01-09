using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.MaterialPurchaseOrders;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.GetSupplierPurchaseOrders;

public sealed class GetSupplierPurchaseOrdersQueryHandler
    : IRequestHandler<GetSupplierPurchaseOrdersQuery, IReadOnlyList<SupplierPurchaseOrderListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetSupplierPurchaseOrdersQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<SupplierPurchaseOrderListItemDto>> Handle(
        GetSupplierPurchaseOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var supplierExists = await _db.Suppliers
            .AnyAsync(s => s.Id == request.SupplierId, cancellationToken);

        if (!supplierExists)
            throw new NotFoundException("Supplier not found");

        return await _db.MaterialPurchaseOrders
            .AsNoTracking()
            .Where(o => o.SupplierId == request.SupplierId)
            .Select(o => new SupplierPurchaseOrderListItemDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                ItemsCount = o.MaterialPurchaseItems.Count,
                TotalAmount = o.MaterialPurchaseItems.Sum(i => i.Qty * i.UnitPrice)
            })
            .OrderByDescending(o => o.OrderDate)
            .ThenBy(o => o.Id)
            .ToListAsync(cancellationToken);
    }
}
