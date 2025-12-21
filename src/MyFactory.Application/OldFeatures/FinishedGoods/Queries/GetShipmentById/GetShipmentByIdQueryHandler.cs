using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Application.Features.FinishedGoods.Common;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Queries.GetShipmentById;

public sealed class GetShipmentByIdQueryHandler : IRequestHandler<GetShipmentByIdQuery, ShipmentDto>
{
    private readonly IApplicationDbContext _context;

    public GetShipmentByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShipmentDto> Handle(GetShipmentByIdQuery request, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments
            .AsNoTracking()
            .Include(entity => entity.Items)
            .FirstOrDefaultAsync(entity => entity.Id == request.ShipmentId, cancellationToken)
            ?? throw new InvalidOperationException("Shipment not found.");

        return await ShipmentDtoFactory.CreateAsync(_context, shipment, cancellationToken);
    }
}
