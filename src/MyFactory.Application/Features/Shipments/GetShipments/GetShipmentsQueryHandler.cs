using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Shipments;
using DomainShipmentStatus = MyFactory.Domain.Entities.Orders.ShipmentStatus;

namespace MyFactory.Application.Features.Shipments.GetShipments;

public sealed class GetShipmentsQueryHandler : IRequestHandler<GetShipmentsQuery, IReadOnlyList<ShipmentListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetShipmentsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ShipmentListItemDto>> Handle(GetShipmentsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Shipments
            .AsNoTracking()
            .Include(s => s.ShipmentItems)
            .AsQueryable();

        if (request.SalesOrderId is Guid salesOrderId)
            query = query.Where(s => s.SalesOrderId == salesOrderId);

        if (request.CustomerId is Guid customerId)
            query = query.Where(s => s.CustomerId == customerId);

        if (request.Status is not null)
        {
            var domainStatus = ShipmentStatusExtensions.ShipmentToDomainStatus(request.Status);

            query = query.Where(s => s.Status == domainStatus);
        }

        if (request.FromDate is not null)
            query = query.Where(s => s.ShipmentDate >= request.FromDate);

        if (request.ToDate is not null)
            query = query.Where(s => s.ShipmentDate <= request.ToDate);

        
        var items = await query
            .Select(s => new
            {
                s.Id,
                s.SalesOrderId,
                s.CustomerId,
                s.ShipmentDate,
                s.Status,
                ItemsCount = s.ShipmentItems.Count,
                TotalQty = s.ShipmentItems.Sum(i => (decimal)i.Qty)
            })
            .ToListAsync(cancellationToken);

        return [.. items
            .Select(s => new ShipmentListItemDto(
                Id: s.Id,
                SalesOrderId: s.SalesOrderId,
                CustomerId: s.CustomerId,
                ShipmentDate: s.ShipmentDate,
                Status: ((DomainShipmentStatus?)s.Status).DomainToShipmentStatus(),
                ItemsCount: s.ItemsCount,
                TotalQty: s.TotalQty))];

    }
}
