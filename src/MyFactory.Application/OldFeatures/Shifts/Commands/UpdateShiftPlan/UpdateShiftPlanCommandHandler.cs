using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Shifts;
using MyFactory.Application.Features.Shifts.Common;

namespace MyFactory.Application.OldFeatures.Shifts.Commands.UpdateShiftPlan;

public sealed class UpdateShiftPlanCommandHandler : IRequestHandler<UpdateShiftPlanCommand, ShiftPlanDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateShiftPlanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShiftPlanDto> Handle(UpdateShiftPlanCommand request, CancellationToken cancellationToken)
    {
        if (request.PlannedQuantity <= 0)
        {
            throw new InvalidOperationException("Planned quantity must be greater than zero.");
        }

        var plan = await _context.ShiftPlans
            .WithDetails()
            .FirstOrDefaultAsync(entity => entity.Id == request.ShiftPlanId, cancellationToken)
            ?? throw new InvalidOperationException("Shift plan not found.");

        plan.UpdateShiftType(request.ShiftType);
        plan.UpdatePlannedQuantity(request.PlannedQuantity);

        await _context.SaveChangesAsync(cancellationToken);

        return await ShiftPlanDtoFactory.CreateAsync(_context, plan, cancellationToken);
    }
}
