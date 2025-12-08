using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Application.Features.FinishedGoods.Common;

namespace MyFactory.Application.Features.FinishedGoods.Queries.GetShipments;

public sealed class GetShipmentsQueryHandler : IRequestHandler<GetShipmentsQuery, IReadOnlyCollection<ShipmentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetShipmentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<ShipmentDto>> Handle(GetShipmentsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Shipments
            .AsNoTracking()
            .Include(shipment => shipment.Items)
            .AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(shipment => shipment.Status == request.Status.Value);
        }

        var shipments = await query
            .OrderByDescending(shipment => shipment.ShipmentDate)
            .ThenBy(shipment => shipment.ShipmentNumber)
            .ToListAsync(cancellationToken);

        return await ShipmentDtoFactory.CreateAsync(_context, shipments, cancellationToken);
    }
}
