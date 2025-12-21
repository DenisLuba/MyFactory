using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Production;
using MyFactory.Application.Features.Production.Common;

namespace MyFactory.Application.OldFeatures.Production.Commands.AllocateProductionOrder;

public sealed class AllocateProductionOrderCommandHandler : IRequestHandler<AllocateProductionOrderCommand, ProductionOrderDto>
{
    private readonly IApplicationDbContext _context;

    public AllocateProductionOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductionOrderDto> Handle(AllocateProductionOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.ProductionOrders
            .WithDetails()
            .FirstOrDefaultAsync(entity => entity.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new InvalidOperationException("Production order not found.");

        var workshopExists = await _context.Workshops
            .AnyAsync(workshop => workshop.Id == request.WorkshopId, cancellationToken);

        if (!workshopExists)
        {
            throw new InvalidOperationException("Workshop not found.");
        }

        var allocation = order.AllocateWorkshop(request.WorkshopId, request.QtyAllocated);
        _context.ProductionOrderAllocations.Add(allocation);
        await _context.SaveChangesAsync(cancellationToken);

        return await ProductionOrderDtoFactory.CreateAsync(_context, order, cancellationToken);
    }
}
