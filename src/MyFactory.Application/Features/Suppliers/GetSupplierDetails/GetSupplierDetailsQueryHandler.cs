using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Suppliers;

namespace MyFactory.Application.Features.Suppliers.GetSupplierDetails;

public sealed class GetSupplierDetailsQueryHandler
    : IRequestHandler<GetSupplierDetailsQuery, SupplierDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetSupplierDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<SupplierDetailsDto> Handle(
        GetSupplierDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var supplier = await _db.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                s => s.Id == request.SupplierId,
                cancellationToken);

        if (supplier is null)
            throw new NotFoundException(
                $"Supplier with Id {request.SupplierId} not found");

        var purchases = await _db.MaterialPurchaseOrders
            .AsNoTracking()
            .Where(o => o.SupplierId == request.SupplierId)
            .Join(
                _db.MaterialPurchaseOrderItems,
                o => o.Id,
                i => i.PurchaseOrderId,
                (o, i) => new { o, i }
            )
            .Join(
                _db.Materials,
                oi => oi.i.MaterialId,
                m => m.Id,
                (oi, m) => new { oi.o, oi.i, m }
            )
            .Join(
                _db.MaterialTypes,
                x => x.m.MaterialTypeId,
                t => t.Id,
                (x, t) => new SupplierPurchaseHistoryDto
                {
                    MaterialType = t.Name,
                    MaterialName = x.m.Name,
                    Qty = x.i.Qty,
                    UnitPrice = x.i.UnitPrice,
                    Date = x.o.OrderDate
                }
            )
            .OrderByDescending(x => x.Date)
            .ToListAsync(cancellationToken);

        return new SupplierDetailsDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Description = supplier.Description,
            Purchases = purchases
        };
    }
}
