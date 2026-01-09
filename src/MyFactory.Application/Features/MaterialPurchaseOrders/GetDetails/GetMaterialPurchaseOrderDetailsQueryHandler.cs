using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.MaterialPurchaseOrders;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.GetDetails;

public sealed class GetMaterialPurchaseOrderDetailsQueryHandler
    : IRequestHandler<GetMaterialPurchaseOrderDetailsQuery, MaterialPurchaseOrderDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetMaterialPurchaseOrderDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<MaterialPurchaseOrderDetailsDto> Handle(
        GetMaterialPurchaseOrderDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _db.MaterialPurchaseOrders
            .AsNoTracking()
            .Include(o => o.MaterialPurchaseItems)
            .Include(o => o.Supplier)
            .FirstOrDefaultAsync(o => o.Id == request.PurchaseOrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException("Purchase order not found");

        return new MaterialPurchaseOrderDetailsDto
        {
            Id = order.Id,
            SupplierId = order.SupplierId,
            SupplierName = order.Supplier?.Name ?? string.Empty,
            OrderDate = order.OrderDate,
            Status = order.Status,
            Items = order.MaterialPurchaseItems
                .Select(i => new MaterialPurchaseOrderItemDto
                {
                    Id = i.Id,
                    MaterialId = i.MaterialId,
                    MaterialName = i.MaterialName,
                    UnitCode = i.UnitCode,
                    Qty = i.Qty,
                    UnitPrice = i.UnitPrice
                })
                .ToList()
        };
    }
}
