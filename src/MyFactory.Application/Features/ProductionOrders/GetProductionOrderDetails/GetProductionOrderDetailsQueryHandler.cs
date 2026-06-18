using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderDetails;

public sealed class GetProductionOrderDetailsQueryHandler : IRequestHandler<GetProductionOrderDetailsQuery, ProductionOrderDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetProductionOrderDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ProductionOrderDetailsDto> Handle(GetProductionOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken);

        var productId = Guid.Empty;
        var productName = string.Empty;
        var departmentName = string.Empty;
        var salesOrderId = Guid.Empty;
        if (po is not null)
        {
            (productId, productName) = await (
                from soi in _db.SalesOrderItems.AsNoTracking()
                join p in _db.Products.AsNoTracking() on soi.ProductId equals p.Id
                where soi.Id == po.SalesOrderItemId
                select new { p.Id, p.Name })
                .FirstOrDefaultAsync(cancellationToken) is { } product
                ? (product.Id, product.Name)
                : (Guid.Empty, string.Empty);

            departmentName = await _db.Departments.AsNoTracking()
                .Where(x => x.Id == po.DepartmentId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

            salesOrderId = await _db.SalesOrderItems.AsNoTracking()
                .Where(x => x.Id == po.SalesOrderItemId)
                .Select(x => x.SalesOrderId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        var result = po is null
            ? throw new NotFoundException("Production order not found")
            : new ProductionOrderDetailsDto
                {
                    Id = po.Id,
                    ProductionOrderNumber = po.ProductionOrderNumber,
                    SalesOrderId = salesOrderId,
                    SalesOrderItemId = po.SalesOrderItemId,
                    ProductId = productId,
                    ProductName = productName,
                    DepartmentId = po.DepartmentId,
                    DepartmentName = departmentName,
                    QtyPlanned = po.QtyPlanned,
                    QtyCut = po.QtyCut,
                    QtySewn = po.QtySewn,
                    QtyPacked = po.QtyPacked,
                    QtyFinished = po.QtyFinished,
                    Status = po.Status
                };

        return result;
    }
}
