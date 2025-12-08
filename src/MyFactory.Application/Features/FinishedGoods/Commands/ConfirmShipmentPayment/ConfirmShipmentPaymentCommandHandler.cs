using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Application.Features.FinishedGoods.Common;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.Features.FinishedGoods.Commands.ConfirmShipmentPayment;

public sealed class ConfirmShipmentPaymentCommandHandler : IRequestHandler<ConfirmShipmentPaymentCommand, ShipmentDto>
{
    private readonly IApplicationDbContext _context;

    public ConfirmShipmentPaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShipmentDto> Handle(ConfirmShipmentPaymentCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments
            .Include(entity => entity.Items)
            .FirstOrDefaultAsync(entity => entity.Id == request.ShipmentId, cancellationToken)
            ?? throw new InvalidOperationException("Shipment not found.");

        if (shipment.Status == ShipmentStatus.Paid)
        {
            return await ShipmentDtoFactory.CreateAsync(_context, shipment, cancellationToken);
        }

        if (shipment.Status != ShipmentStatus.Shipped)
        {
            throw new InvalidOperationException("Only shipped shipments can be marked as paid.");
        }

        shipment.MarkAsPaid();

        await _context.SaveChangesAsync(cancellationToken);

        return await ShipmentDtoFactory.CreateAsync(_context, shipment, cancellationToken);
    }
}
