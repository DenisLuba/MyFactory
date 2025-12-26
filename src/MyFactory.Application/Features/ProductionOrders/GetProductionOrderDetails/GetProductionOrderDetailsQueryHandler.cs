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
        if (po is null)
            throw new NotFoundException("Production order not found");
        return new ProductionOrderDetailsDto
        {
            Id = po.Id,
            ProductionOrderNumber = po.ProductionOrderNumber,
            SalesOrderItemId = po.SalesOrderItemId,
            DepartmentId = po.DepartmentId,
            QtyPlanned = po.QtyPlanned,
            QtyCut = po.QtyCut,
            QtySewn = po.QtySewn,
            QtyPacked = po.QtyPacked,
            QtyFinished = po.QtyFinished,
            Status = po.Status
        };
    }
}
