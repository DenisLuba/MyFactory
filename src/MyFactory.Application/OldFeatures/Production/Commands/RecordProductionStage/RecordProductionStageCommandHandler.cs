using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Production;
using MyFactory.Application.Features.Production.Common;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.OldFeatures.Production.Commands.RecordProductionStage;

public sealed class RecordProductionStageCommandHandler : IRequestHandler<RecordProductionStageCommand, ProductionOrderDto>
{
    private readonly IApplicationDbContext _context;

    public RecordProductionStageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductionOrderDto> Handle(RecordProductionStageCommand request, CancellationToken cancellationToken)
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

        var stage = order.ScheduleStage(request.WorkshopId, request.StageType);
        _context.ProductionStages.Add(stage);
        stage.Start(request.QtyIn, request.RecordedAt);

        if (order.Status == ProductionOrderStatuses.Planned)
        {
            order.Start();
        }

        if (request.QtyOut > 0)
        {
            stage.Complete(request.QtyOut, request.RecordedAt);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return await ProductionOrderDtoFactory.CreateAsync(_context, order, cancellationToken);
    }
}
