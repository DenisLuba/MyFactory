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
            .Where(materialPurchaseOrder => materialPurchaseOrder.SupplierId == request.SupplierId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new SupplierPurchaseHistoryDto
            {
                OrderId = o.Id,
                PurchaseNumber = o.PurchaseNumber,
                Date = o.OrderDate,
                Status = o.Status,
                Items = o.MaterialPurchaseItems
                    .Select(i => new SupplierPurchaseHistoryItemDto(
                        i.Material != null 
                            ? (i.Material.MaterialType != null ? i.Material.MaterialType.Name : string.Empty) 
                            : string.Empty,
                        i.Material != null 
                            ? i.Material.Name 
                            : string.Empty,
                        i.Qty,
                        i.UnitPrice))
                    .ToList()
            })
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
